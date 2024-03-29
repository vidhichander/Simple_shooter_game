﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
     #region Editor Variables
    [SerializeField]
    [Tooltip("Text component housing current high score")]
    private Text m_HighScore; 

    #endregion

    #region Private Variables
    private string m_DefaultHighScoreText;
    #endregion

    #region Initialization 
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
         
    }
    private void Start()
    {
        m_DefaultHighScoreText = m_HighScore.text;
        UpdateHighScore();

    }

    #endregion

    #region Play Button Methods
    public void PlayArena()
    {
        SceneManager.LoadScene("Level 1");
    }
    #endregion
      
    #region General Application Button Methods
    public void Quit()
    {
        Application.Quit(); 
    }

    public void Rules()
    {
        SceneManager.LoadScene("Rules");
    }
    #endregion

    #region HighScore Methods
    private void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("HS"))
        {
            m_HighScore.text = m_DefaultHighScoreText.Replace("%S", PlayerPrefs.GetInt("HS").ToString());
        }
        else
        {
            PlayerPrefs.SetInt("HS", 0);
            m_HighScore.text = m_DefaultHighScoreText.Replace("%S", "0");
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HS", 0);
        UpdateHighScore(); 
    }
    #endregion
}
