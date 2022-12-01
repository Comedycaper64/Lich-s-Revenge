using System.Collections;
using System.Collections.Generic;
using Units.Enemy.Miner;
using UnityEngine;

public class MinerWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private MinerWeaponHandler handler;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private int damage;
    private float knockback;

    public void SetHandler(MinerWeaponHandler handler)
    {
        this.handler = handler;
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
        if (other ==  myCollider) {return;}

        if (alreadyCollidedWith.Contains(other)) {return;}

        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            //End attack, weapon bounce state, or something similar
            handler.DisableWeapon();
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
