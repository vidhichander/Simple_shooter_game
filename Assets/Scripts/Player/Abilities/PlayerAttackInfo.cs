using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackInfo 
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("A name for this attack")]
    private string m_Name;
    public string AttackName
    {
        get
        {
            return m_Name;
        }
    }

    [SerializeField]
    [Tooltip("The button to press in order to use this attack. This button must be in Input Settings")]
    private string m_Button;
    public string Button
    {
        get
        {
            return m_Button; 
        }
    }

    [SerializeField]
    [Tooltip("Trigger string to use to activate this attack in animator")]
    private string m_TriggerName;
    public string TriggerName
    {
        get
        {
            return m_TriggerName;
        }
    }

    [SerializeField]
    [Tooltip("Prefab of game object representing the ability")]
    private GameObject m_AbilityGO;
    public GameObject AbilityGO
    {
        get
        {
            return m_AbilityGO;
        }
    }

    [SerializeField]
    [Tooltip("Where to spawn the ability game object with respect to player")]
    private Vector3 m_offset;
    public Vector3 offset
    {
        get
        {
            return m_offset;
        }
    }

    [SerializeField]
    [Tooltip("How much time before this attack should be activated after button is pressed")]
    private float m_WindUpTime;
    public float WindUpTime
    {
        get
        {
            return m_WindUpTime;
        }
    }

    [SerializeField]
    [Tooltip("Time before player can do action again")]
    private float m_FrozenTime;
    public float FrozenTime
    {
        get
        {
            return m_FrozenTime;
        }
    }

    [SerializeField]
    [Tooltip("How long to wait before this ability can be used again by player")]
    private float m_Cooldown;

    [SerializeField]
    [Tooltip("How much health player will lose using this ability")]
    private int m_HealthCost;
    public int HealthCost
    {
        get
        {
            return m_HealthCost;
        }
    }

    [SerializeField]
    [Tooltip("Color to change to when using this ability")]
    private Color m_Color;
    public Color AbilityColor
    {
        get
        {
            return m_Color;
        }
    }
    #endregion

    #region Public Variables
    public float Cooldown
    {
        get;
        set;
    }
    #endregion

    #region Cooldown Methods
    public void ResetCooldown()
    {
        Cooldown = m_Cooldown;
    }

    public bool IsReady()
    {
        return Cooldown <= 0;
    }
    #endregion
}
