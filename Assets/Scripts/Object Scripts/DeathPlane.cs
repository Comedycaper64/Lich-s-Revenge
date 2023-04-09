using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    //A plane places below all levels. On contact with the player or an enemy, kills them

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(999f);
        }    
    }
}
