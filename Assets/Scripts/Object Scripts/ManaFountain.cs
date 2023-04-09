using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class ManaFountain : MonoBehaviour
{
    //Heals the player when they get close to this object. Also sets a respawn point for if the player dies
    [SerializeField] private Transform respawnTransform;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine playerStateMachine))
        {
            other.GetComponent<Health>().Heal(999f);
            playerStateMachine.SetRespawnPoint(respawnTransform);
        }
    }

}
