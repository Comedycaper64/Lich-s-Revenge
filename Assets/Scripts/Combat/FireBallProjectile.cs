using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    private Collider playerCollider;
    private int damage;
    private float knockback;

    private float animationTime = 2f;
    [SerializeField] private float timeToLive;
    private float projectileSpeed;
    [SerializeField] private float fireballExplodeRadius;
    [SerializeField] private GameObject fireballVFX;

    private void Start() 
    {
        StartCoroutine(SeeIfLaunched());
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

    public void SetProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void SetPlayerCollider(Collider playerCollider)
    {
        this.playerCollider = playerCollider;
    }
    
    private IEnumerator SeeIfLaunched()
    {
        yield return new WaitForSeconds(animationTime);
        if (projectileSpeed == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, timeToLive);
        }
    }

    private void ExplodeFireball()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, fireballExplodeRadius);

        //Explode SFX
        GameObject explosion = Instantiate(fireballVFX, transform.position, Quaternion.identity);
        Destroy(explosion, 3f);

        foreach(Collider collider in colliders)
        {
            if (collider.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage);
            }

            if (collider.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                forceReceiver.AddForce((collider.transform.position - playerCollider.transform.position).normalized * knockback);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other ==  playerCollider) {return;}

        //when collides with health or layer 6 collider, do damage in sphere with radius based on stats


        if(other.TryGetComponent<Health>(out Health health) || other.gameObject.layer == 6)
        {
            ExplodeFireball();
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((other.transform.position - playerCollider.transform.position).normalized * knockback);
            Destroy(gameObject);
        }
    }
}
