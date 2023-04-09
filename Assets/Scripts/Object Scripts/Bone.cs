using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class Bone : MonoBehaviour
{
    //Object dropped when an enemy dies. If the player walks over it, their bone resource is incremented.
    private void Awake() 
    {
        PlayerStateMachine.OnRespawn += PlayerRespawned;    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<LichBones>(out LichBones lichBones))
        {
            if (lichBones.TryAddBone())
            {
                Destroy(gameObject);
            }
        }    
    }

    //If the player respawns, any bones in the level are destroyed
    private void PlayerRespawned()
    {
        Destroy(gameObject);
    }

    private void OnDestroy() 
    {
        PlayerStateMachine.OnRespawn -= PlayerRespawned;    
    }
}
