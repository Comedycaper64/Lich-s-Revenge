using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script attached to the Hammerer's hammer
public class HammererWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private HammererWeaponHandler handler;
    private Health unitHealth;
    private ForceReceiver unitForceReceiver;
    private Collider unitCollider;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private float damage;
    private float knockback;
    [SerializeField] private AudioClip lichHitSFX;

    public void SetHandler(HammererWeaponHandler handler)
    {
        this.handler = handler;
        unitHealth = handler.GetComponent<Health>();
        unitForceReceiver = handler.GetComponent<ForceReceiver>();
        unitCollider = handler.GetComponent<Collider>();
    }

    public void SetAttack(float damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }

    private void OnEnable() 
    {
        alreadyCollidedWith.Clear();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        TryAttack(other);
    }

    //Very similar to the MinerWeapon. Deals damage on contact with the player
    private void TryAttack(Collider collider)
    {
        if (collider ==  myCollider) {return;}

        if (alreadyCollidedWith.Contains(collider)) {return;}

        alreadyCollidedWith.Add(collider);

        if(collider.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            handler.DisableWeapon();
            unitHealth.DealDamage(0);
            unitForceReceiver.AddForce((unitCollider.transform.position - collider.transform.position).normalized * knockback);
            aegis.DamageAegis();
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(lichHitSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
        }

        if(collider.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(lichHitSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
        }

        if (collider.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((collider.transform.position - myCollider.transform.position).normalized * knockback);
        }
    }

    //Slightly different behaviour than above for the Slam Attack
    public void TrySlamAttack(Collider collider)
    {
        if(collider.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            aegis.DamageAegis();
        }
        else
        {
            if(collider.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage);
            }

            if (collider.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                forceReceiver.AddForce((collider.transform.position - myCollider.transform.position).normalized * knockback);
            }
        }
    }
}
