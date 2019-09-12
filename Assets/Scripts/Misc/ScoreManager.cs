using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //keeps track of score without resetting each time game is started
    public static ScoreManager singleton;

    #region Private Variables
    private int m_CurScore;
    #endregion

    //checking if this is only singleton in script
    #region Initialization
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;

        } else if (singleton == this)
        {
            Destroy(gameObject);
        }
        m_CurScore = 0;
    }
    #endregion

    #region Score Methods
    public void IncreaseScore(int amount)
    {
        m_CurScore += amount;
    }

    public void UpdateHighScore()
    {
        //playerprefs = dictionary of values that gets saved in game data -- persists between scenes and exits. Can store int, float, and bool
        if (!PlayerPrefs.HasKey("HS")){
            PlayerPrefs.SetInt("HS", m_CurScore);
        }

        int hs = PlayerPrefs.GetInt("HS");
        if (m_CurScore > hs)
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
