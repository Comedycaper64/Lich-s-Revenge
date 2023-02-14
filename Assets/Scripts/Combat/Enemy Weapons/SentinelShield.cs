using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelShield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<FireBoltProjectile>() || other.GetComponent<FireBallProjectile>())
        {
            other.transform.Rotate(new Vector3(0, 180, 0));
        }    
    }
}
