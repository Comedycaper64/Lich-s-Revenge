using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class Bone : MonoBehaviour
{
    private void Awake() 
    {
        PlayerStateMachine.OnRespawn += PlayerRespawned;    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<LichBones>(out LichBones lichBones))
        {
            lichBones.AddBone();
            Destroy(gameObject);
        }    
    }

    private void PlayerRespawned()
    {
        Destroy(gameObject);
    }

    private void OnDestroy() 
    {
        PlayerStateMachine.OnRespawn -= PlayerRespawned;    
    }
}
