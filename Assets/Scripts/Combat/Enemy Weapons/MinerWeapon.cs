using System.Collections;
using System.Collections.Generic;
using Units.Enemy.Miner;
using UnityEngine;

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

    private void OnTriggerEnter(Collider other) 
    {
        if (other ==  myCollider) {return;}

        if (alreadyCollidedWith.Contains(other)) {return;}

        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            handler.DisableWeapon();
            unitHealth.DealDamage(0);
            unitForceReceiver.AddForce((unitCollider.transform.position - other.transform.position).normalized * knockback);
            aegis.DamageAegis(damage, false);

            //End attack, weapon bounce state, or something similar
            // if (aegis.IsParrying())
            // {
            //     handler.DisableWeapon();
            //     unitHealth.DealDamage(0);
            //     unitForceReceiver.AddForce((unitCollider.transform.position - other.transform.position).normalized * knockback);
            // }
            // else
            // {
            //     aegis.DamageAegis(damage);
            //     handler.DisableWeapon();
            // }
        }

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((other.transform.position - myCollider.transform.position).normalized * knockback);
        }
    }
}
