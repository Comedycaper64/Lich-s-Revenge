using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampPlane : MonoBehaviour
{
    //A surface used in the second level. Tracks what entities enter the swamp. Damages them while they're in it
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

    //The entity list is traversed in reverse
    //This is to avoid an error in case an entity is removed due to being killed from the damage.
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
