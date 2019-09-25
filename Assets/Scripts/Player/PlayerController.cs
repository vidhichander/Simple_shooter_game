using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    #region Editor Variables
    //Allows user to edit fields in unity editor
    [SerializeField]
    [Tooltip("How fast the player should move when running around.")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("The transform of the camera following the player")]
    private Transform m_CameraTransform;

    [SerializeField]
    [Tooltip("List of attacks with info about them")]
    private PlayerAttackInfo[] m_Attacks;

    [SerializeField]
    [Tooltip("Amount of health player starts with")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("The HUD Script")]
    private HUDController m_HUD;

    [SerializeField]
    [Tooltip("How fast the player should move when running around.")]
    private Text m_KillCountDown;

    #endregion

    #region Cached References
    private Animator cr_Anim;
    private Renderer cr_Renderer;
    #endregion
    //region that holds variables that are components from host object
    #region Cached Components 
    private Rigidbody cc_Rb;
    #endregion

    #region Private Variables
    // The current move direction of the player. DOES NOT include magnitude
    private Vector2 p_Velocity;

    //In order to do anything, player cannot be frozen (timer must be 0)
    private float p_FrozenTimer;

    //Keep track of default color
    private Color p_DefaultColor;

    //Current amount of health player has (damage taken not full numbers)
    private float p_CurHealth;

    private int p_CurScore;

    public float p_KillTime;
    #endregion

    #region Initialization
    private void Awake()
    {
        m_KillCountDown.text = "";
        p_KillTime = 0;
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
        cr_Anim = GetComponent<Animator>();
        cr_Renderer = GetComponentInChildren<Renderer>();
        p_DefaultColor = cr_Renderer.material.color;
        p_CurHealth = m_MaxHealth;

        p_FrozenTimer = 0;

        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];
            attack.Cooldown = 0;

            if (attack.WindUpTime > attack.FrozenTime)
            {
                Debug.LogError(attack.AttackName + " has a wind up time that is larger than amount of time player is frozen for");
            }
        }
        p_CurScore = 0;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion

    #region Main Updates
    private void Update()
    {
        if (p_KillTime > 0)
        {
            p_KillTime -= Time.deltaTime;
            m_KillCountDown.text = "Killer Mode Activated! Time Left = " + p_KillTime;
        }
        else
        {
            m_KillCountDown.text = "";
        }

        if (p_FrozenTimer > 0)
        {
            p_Velocity = Vector2.zero;
            p_FrozenTimer -= Time.deltaTime;
            return;
        }
        else
        {
            p_FrozenTimer = 0;
        }
        //Ability use -- see if ready
        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];

            if (attack.IsReady())
            {
                if (Input.GetButtonDown(attack.Button))
                {
                    p_FrozenTimer = attack.FrozenTime;
                    DecreaseHealth(attack.HealthCost);
                    StartCoroutine(UseAttack(attack));
                    break;
                }
            } else if (attack.Cooldown > 0)
            {
                attack.Cooldown -= Time.deltaTime;
            }


        }


        // see how hard player is pressomg movement buttons
        // up and down (+ and -)
        float forward = Input.GetAxis("Vertical");

        //right and left(+ and -)
        float right = Input.GetAxis("Horizontal");

        //updating the animation
        cr_Anim.SetFloat("Speed", Mathf.Clamp01(Mathf.Abs(forward) + Mathf.Abs(right)));

        // velocity
        float moveThreshold = 0.3f;

        if (forward > 0 && forward < moveThreshold)
        {
            forward = 0;
        } else if (forward < 0 && forward > -moveThreshold)
        {
            forward = 0;
        }

        if (right > 0 && right < moveThreshold)
        {
            right = 0; 
        } else if (right < 0 && right > -moveThreshold)
        {
            right = 0;
        }
        p_Velocity.Set(right, forward);
        }

    //for updates using physics. Runs on a set time frame.
    private void FixedUpdate()
    {
        //Update player position
        cc_Rb.MovePosition(cc_Rb.position + m_Speed * Time.fixedDeltaTime * transform.forward * p_Velocity.magnitude);

        //Update player rotation
        cc_Rb.angularVelocity = Vector3.zero;
        if (p_Velocity.sqrMagnitude > 0)
        {
            float angletoRotCam = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            Vector3 camForward = m_CameraTransform.forward;
            Vector3 newRot = new Vector3(Mathf.Cos(angletoRotCam) * camForward.x - Mathf.Sin(angletoRotCam) * camForward.z, 0, Mathf.Cos(angletoRotCam) * camForward.z + Mathf.Sin(angletoRotCam) * camForward.x);
            float theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);
        }
    }

    #endregion

    #region Health/Dying Methods
    public void DecreaseHealth(float amount)
    {
        p_CurHealth -= amount;
        m_HUD.UpdateHealth(1.0f * p_CurHealth / m_MaxHealth);
        if (p_CurHealth <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void IncreaseHealth(float amount)
    {
        p_CurHealth += amount;
        if (p_CurHealth > m_MaxHealth)
        {
            p_CurHealth = m_MaxHealth;
        }
        m_HUD.UpdateHealth(1.0f * p_CurHealth / m_MaxHealth);
    }
    #endregion

    #region Attack Methods
    private IEnumerator UseAttack(PlayerAttackInfo attack)
    {

        cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        cr_Anim.SetTrigger(attack.TriggerName);
        IEnumerator toColor = ChangeColor(attack.AbilityColor, 10);
        StartCoroutine(toColor);
        yield return new WaitForSeconds(attack.WindUpTime);

        Vector3 offset = transform.forward * attack.offset.z + transform.right * attack.offset.x + transform.up * attack.offset.y;
        GameObject go = Instantiate(attack.AbilityGO, transform.position + offset, cc_Rb.rotation);
        go.GetComponent<Ability>().Use(transform.position + offset);

        StopCoroutine(toColor);
        StartCoroutine(ChangeColor(p_DefaultColor, 50)); 
        yield return new WaitForSeconds(attack.Cooldown);
        attack.ResetCooldown(); 
    }
    #endregion

    //Coroutine to change player's colors
    #region Misc Methods
    private IEnumerator ChangeColor(Color newColor, float speed)
    {
        Color curColor = cr_Renderer.material.color;
        while (curColor != newColor)
        {
            curColor = Color.Lerp(curColor, newColor, speed / 100);
            cr_Renderer.material.color = curColor;
            yield return null;
        }
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPill"))
        {
            IncreaseHealth(other.GetComponent<HealthPill>().HealthGain);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("KillPill"))
        {
            p_KillTime = 10;
            m_KillCountDown.text = "Killer Mode Activated! Time Left = " + p_KillTime;
            Destroy(other.gameObject);

        }
    }
    #endregion
}
