using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Enemy.Gunner;
using Units.Player;
using UnityEngine;

public class GunnerWeaponHandler : MonoBehaviour
{
    private Transform playerTransform;
    private DwarfGunnerStats gunnerStats;
    private Vector3 playerDirection;
    [SerializeField] private Transform weaponTransform;  
    [SerializeField] private GameObject blastVFX;  

    private void Awake() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        gunnerStats = GetComponent<DwarfGunnerStats>();
    }

    public void FireWeapon()
    {
        playerDirection = (GetPlayerPosition() - weaponTransform.position).normalized;
        float playerDistance = (GetPlayerPosition() - weaponTransform.position).magnitude;
        Destroy(Instantiate(blastVFX, weaponTransform.position, gunnerStats.transform.rotation), 2f);
        if ((playerDistance < gunnerStats.GetAttackRange()) && (Vector3.Dot(playerDirection, transform.forward) > gunnerStats.GetAttackArc()))
        {
            float damage = gunnerStats.GetAttack();
            float knockback = gunnerStats.GetAttack() * 2;    
            if(playerTransform.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine))
            {
                if (stateMachine.Aegis.absorbing)
                {
                    stateMachine.Aegis.DamageAegis();
                }
                else
                {
                    if(playerTransform.TryGetComponent<Health>(out Health health))
                    {
                        health.DealDamage(damage);
                    }

                    if (playerTransform.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
                    {
                        forceReceiver.AddForce((GetPlayerPosition() - GetComponent<Collider>().transform.position).normalized * knockback);
                    }
                }
            }   
        }
    }

    private Vector3 GetPlayerPosition()
    {
        Vector3 playerPosition = playerTransform.position;
        playerPosition.y += 0.9f;
        return playerPosition;
    }
}
