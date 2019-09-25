using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{


    //keeps track of score without resetting each time game is started
    public static ScoreManager singleton;

    #region Private Variables
    private int m_CurScore;
    private int m_level;
    public int level
    {
        get
        {
            return m_level;
        }
    }

    #endregion

    #region Editor Variables
    [SerializeField]
    [Tooltip("Score")]
    private Text m_Score;

    [SerializeField]
    [Tooltip("HighScore")]
    private Text m_HighSc;


    [SerializeField]
    [Tooltip("Level up")]
    private Text m_levelUp;
    #endregion


    //checking if this is only singleton in script
    #region Initialization
    private void Awake()
    {
        m_level = 1;
        m_levelUp.text = "LEVEL " + m_level;
        if (singleton == null)
        {
            singleton = this;

        } else if (singleton != this)
        {
            Destroy(gameObject);
        }
        m_CurScore = 0;
        if (!PlayerPrefs.HasKey("HS"))
        {
            m_HighSc.text = "HighScore: 0";
        }
        else
        {
            m_HighSc.text = "HighScore: " + PlayerPrefs.GetInt("HS");
        }
        UpdateScore();
    }
    #endregion

    #region Score Methods
    public void IncreaseScore(int amount)
    {

        m_CurScore += amount;
        if (m_CurScore >= m_level * 20)
        {
            m_level += 1;
            m_levelUp.text = "LEVEL " + m_level;

        }
        UpdateScore();
    }

    private void UpdateScore()
    {
        m_Score.text = "Score: " + m_CurScore;
    }

    private void UpdateHighScore()
    {
        //playerprefs = dictionary of values that gets saved in game data -- persists between scenes and exits. Can store int, float, and bool
        if (!PlayerPrefs.HasKey("HS")){
            PlayerPrefs.SetInt("HS", m_CurScore);
        }

        int hs = PlayerPrefs.GetInt("HS");
        if (hs < m_CurScore)
        {
            PlayerPrefs.SetInt("HS", m_CurScore);
        }
    }

    #endregion

    #region Destruction
    private void OnDisable()
    {
        UpdateHighScore();
    }
    #endregion
}
