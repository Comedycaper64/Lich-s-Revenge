using System;
using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class BoneMine : MonoBehaviour
{
    // An object instantiated when the player sets a mine. Explodes upon contact with an enemy
    private float mineExplodeRadius;
    [SerializeField] private GameObject fireballVFX;
    private float damage;
    [SerializeField] private AudioClip fireballExplodeSFX;
    [SerializeField] private AudioClip boneSetSFX;

    private void Start() 
    {
        PlayerStateMachine.OnRespawn += DestroyMine;    
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(boneSetSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine stateMachine)) {return;}

        //when collides with health or layer 6 collider, do damage in sphere with radius based on stats

        if(other.TryGetComponent<Health>(out Health health))
        {
            ExplodeFireball();
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage; 
    }

    public void SetExplodeRadius(float explodeRadius)
    {
        mineExplodeRadius = explodeRadius;
    }

    //Similar method for dealing damage as with the Fireball Projectile
    private void ExplodeFireball()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, mineExplodeRadius);

        GameObject explosion = Instantiate(fireballVFX, transform.position, Quaternion.identity);
        Destroy(explosion, 3f);

        foreach(Collider collider in colliders)
        {
            //Damages enemies, not player
            if (collider.TryGetComponent<Health>(out Health health))
            {
                if (collider.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine playerStateMachine))
                {
                    continue;
                }
                else
                {
                    health.DealDamage(damage);
                }
            }

            //Knocks enemies back
            if (collider.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                forceReceiver.AddForce((collider.transform.position - transform.position).normalized * damage);
            }
        }
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireballExplodeSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
        Destroy(gameObject);
    }

    private void DestroyMine()
    {
        Destroy(gameObject);
    }

    private void OnDestroy() 
    {
        PlayerStateMachine.OnRespawn -= DestroyMine; 
    }
}
