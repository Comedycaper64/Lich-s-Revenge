using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(999f);
        }    
    }
}
