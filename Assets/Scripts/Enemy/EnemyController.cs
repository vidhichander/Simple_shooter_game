using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    #region Editor Variables
    [SerializeField]
    [Tooltip("Amt of health")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("Speed of the enemy")]
    private float m_speed;


    [SerializeField]
    [Tooltip("Approx damage dealt per frame")]
    private float m_Damage;

    [SerializeField]
    [Tooltip("The explosion that happens when enemy dies")]
    private ParticleSystem m_DeathExplosion;

    [SerializeField]
    [Tooltip("Probability that this enemy drops Health Pill")]
    private float m_HealthPillDropRate;

    [SerializeField]
    [Tooltip("Type of health pill dropped")]
    private GameObject m_HealthPill;

    [SerializeField]
    [Tooltip("Probability that this enemy drops Flying Pill")]
    private float m_KillPillDropRate;

    [SerializeField]
    [Tooltip("Type of Flying pill dropped")]
    private GameObject m_KillPill;

    [SerializeField]
    [Tooltip("How many points will player get for killing this enemy")]
    private int m_Score;

    #endregion 

    #region Private Variables
    private float p_curHealth;
    #endregion

    //part of following player -- keeping track of enemy's rigidbody
    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    // Follows the player
    #region Cached References
    private Transform cr_Player;
    #endregion

    #region Initialization
    private void Awake()
    { 
        m_speed += ScoreManager.singleton.level;


        p_curHealth = m_MaxHealth;
        cc_Rb = GetComponent<Rigidbody>();

    }
    private void Start()
    {
        cr_Player = FindObjectOfType<PlayerController>().transform;

    }

    #endregion

    #region Main Updates
    private void FixedUpdate()
    {
        Vector3 dir = cr_Player.position - transform.position;
        dir.Normalize();
        cc_Rb.MovePosition(cc_Rb.position + dir * m_speed * Time.fixedDeltaTime);
    }
    #endregion

    #region Collision Methods

    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.collider.gameObject;
         if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().p_KillTime > 0)
            {
                DecreaseHealth(50000);
            }
            else
            {
                other.GetComponent<PlayerController>().DecreaseHealth(m_Damage);

            }
        }
    }

    #endregion

    #region Health Methods
    public void DecreaseHealth(float amount)
    {
        p_curHealth -= amount;
        if (p_curHealth <= 0)
        {

            ScoreManager.singleton.IncreaseScore(m_Score);
            int x = Random.Range(0, 2);
            if (x == 0)
            {
                if (Random.value < m_HealthPillDropRate)
                {
                    Instantiate(m_HealthPill, transform.position, Quaternion.identity);
                }
            }
            else
            {
                if (Random.value < m_KillPillDropRate)
                {
                    Instantiate(m_KillPill, transform.position, Quaternion.identity);
                }
            }

            Instantiate(m_DeathExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
    #endregion

}
