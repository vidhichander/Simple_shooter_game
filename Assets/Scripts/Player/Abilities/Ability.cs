//Abstract Class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour 
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("All the main info about this ability")]
    protected AbilityInfo m_Info;
    #endregion

    #region Cached Components
    protected ParticleSystem cc_Ps;
    #endregion

    #region Initialization
    private void Awake()
    {
        cc_Ps = GetComponent<ParticleSystem>();
    }
    #endregion

    #region Use Methods
    public abstract void Use(Vector3 spawnPos);
    #endregion
}
