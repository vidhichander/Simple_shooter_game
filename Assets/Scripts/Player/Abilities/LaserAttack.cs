using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : Ability
{
    public override void Use(Vector3 spawnPos)
    {
        //Stores an object hit by the raycast - multiple objects use an array.
        RaycastHit hit;
        float newlength = m_Info.Range;
        if (Physics.SphereCast(spawnPos, 0.5f, transform.forward, out hit, m_Info.Range)){
            newlength = (hit.point - spawnPos).magnitude;
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>().DecreaseHealth(m_Info.Power); 
            }

        }
        var emitterShape = cc_Ps.shape;
        emitterShape.length = newlength;
        cc_Ps.Play();

    }
}
