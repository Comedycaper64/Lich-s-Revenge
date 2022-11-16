using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    private Collider playerCollider;
    private int damage;
    private float knockback;

    [SerializeField] private float timeToLive;
    [SerializeField] private float projectileSpeed;

    private void Start() 
    {
        Destroy(gameObject, timeToLive);
    }

    private void Update() 
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);    
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }

    public void SetPlayerCollider(Collider playerCollider)
    {
        this.playerCollider = playerCollider;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other ==  playerCollider) {return;}

        //when collides with health or layer 6 collider, do damage in sphere with radius based on stats


        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((other.transform.position - playerCollider.transform.position).normalized * knockback);
            Destroy(gameObject);
        }
    }
}
