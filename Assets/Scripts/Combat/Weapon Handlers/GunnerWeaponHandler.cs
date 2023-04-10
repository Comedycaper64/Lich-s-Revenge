using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Enemy.Gunner;
using Units.Player;
using UnityEngine;

//Script used for the Gunner's attacks
public class GunnerWeaponHandler : MonoBehaviour
{
    private Transform playerTransform;
    private DwarfGunnerStats gunnerStats;
    private Vector3 playerDirection;
    [SerializeField] private Transform weaponTransform;  
    [SerializeField] private GameObject blastVFX;  
    [SerializeField] private AudioClip blastSFX;

    private void Awake() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        gunnerStats = GetComponent<DwarfGunnerStats>();
    }

    //One of the few instances where an attack does not deal damage via collision
    //When the gunner's weapon is fired, it tests to see if the player is within the gun's attack range and attack arc. If so, then it deals damage
    public void FireWeapon()
    {
        playerDirection = (GetPlayerPosition() - weaponTransform.position).normalized;
        float playerDistance = (GetPlayerPosition() - weaponTransform.position).magnitude;
        Destroy(Instantiate(blastVFX, weaponTransform.position, gunnerStats.transform.rotation), 2f);
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(blastSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
        if ((playerDistance < gunnerStats.GetAttackRange()) && (Vector3.Dot(playerDirection, transform.forward) > gunnerStats.GetAttackArc()))
        {
            float damage = gunnerStats.GetAttack();
            float knockback = gunnerStats.GetAttack() * 2;    
            if(playerTransform.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine))
            {
                //Doesn't deal damage if the player is using the Absorb ability
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
