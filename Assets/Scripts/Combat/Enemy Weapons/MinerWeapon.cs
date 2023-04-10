using System.Collections;
using System.Collections.Generic;
using Units.Enemy.Miner;
using UnityEngine;

//Script attached to the Miner's pickaxe
public class MinerWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private MinerWeaponHandler handler;
    private Health unitHealth;
    private ForceReceiver unitForceReceiver;
    private Collider unitCollider;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private float damage;
    private float knockback;
    [SerializeField] private AudioClip lichHitSFX;

    public void SetHandler(MinerWeaponHandler handler)
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

    //Damages the player on contact with them. Includes alternate behaviour used in scenario 5, where the player has access to a new mechanic
    private void OnTriggerEnter(Collider other) 
    {
        if (other ==  myCollider) {return;}

        if (alreadyCollidedWith.Contains(other)) {return;}

        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            if (aegis.blocking)
            {
                handler.DisableWeapon();
                unitHealth.DealDamage(30f);
                unitForceReceiver.AddForce((unitCollider.transform.position - other.transform.position).normalized * knockback * 3);
            }
            else
            {
                handler.DisableWeapon();
                unitHealth.DealDamage(0);
                unitForceReceiver.AddForce((unitCollider.transform.position - other.transform.position).normalized * knockback);
                aegis.DamageAegis();                
            }
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(lichHitSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
        }

        if(other.TryGetComponent<Health>(out Health health) && other.gameObject.layer != 7)
        {
            health.DealDamage(damage);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(lichHitSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((other.transform.position - myCollider.transform.position).normalized * knockback);
        }
    }
}
