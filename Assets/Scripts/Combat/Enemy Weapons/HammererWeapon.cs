using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammererWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private HammererWeaponHandler handler;
    private Health unitHealth;
    private ForceReceiver unitForceReceiver;
    private Collider unitCollider;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private int damage;
    private float knockback;

    public void SetHandler(HammererWeaponHandler handler)
    {
        this.handler = handler;
        unitHealth = handler.GetComponent<Health>();
        unitForceReceiver = handler.GetComponent<ForceReceiver>();
        unitCollider = handler.GetComponent<Collider>();
    }

    public void SetAttack(int damage, float knockback)
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
            aegis.DamageAegis(999f, false);
        }

        if(collider.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }

        if (collider.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((collider.transform.position - myCollider.transform.position).normalized * knockback);
        }
    }

    public void TrySlamAttack(Collider collider)
    {
        if(collider.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            aegis.DamageAegis(damage, false);
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
