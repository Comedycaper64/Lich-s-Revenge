using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampPlane : MonoBehaviour
{
    private List<Health> submergedEntities = new List<Health>();

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            submergedEntities.Add(health);
        } 
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            submergedEntities.Remove(health);
        }
    }

    private void Update() 
    {
        for(int i = submergedEntities.Count - 1; i >= 0; i--)
        {
            submergedEntities[i].DealDamage(30f);
            if (submergedEntities[i].isDead)
            {
                submergedEntities.Remove(submergedEntities[i]);
            }
        }    
    }
}
