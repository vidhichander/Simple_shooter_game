using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows system to be embedded in another class. We are using AbilityInfo as a datastructure
[System.Serializable]
public class AbilityInfo 
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How much power does this ability have")]
    private int m_Power;
    public int Power
    {
        get
        {
            return m_Power;
        }
    }

    [SerializeField]
    [Tooltip("If this attack shoots something out, this value describes how far it can shoot")]
    private int m_Range;
    public int Range
    {
        get
        {
            return m_Range;
        }
    }

    #endregion

}
