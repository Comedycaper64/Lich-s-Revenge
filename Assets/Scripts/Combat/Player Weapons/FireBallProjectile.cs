using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    [SerializeField] private Collider fireballCollider;
    private Collider playerCollider;
    private float damage;
    private float knockback;

    private bool damagePlayer;

    private float animationTime = 2f;
    private float timeToLive;
    private float projectileSpeed;
    [SerializeField] private GameObject fireballVFX;
    [SerializeField] private AudioClip fireballExplodeSFX;

    private float fireballExplodeRadius;

    private float detonationTime = 0f;

    private void Start() 
    {
        EnableCollider(false);
        StartCoroutine(SeeIfLaunched());
    }

    private void Update() 
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);    
        if (detonationTime > 0f)
        {
            detonationTime -= Time.deltaTime;
            if (detonationTime <= 0f)
            {
                detonationTime = 0f;
                ExplodeFireball();
            }
        }
    }

    public void SetAttack(float damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }

    public void SetDamagePlayer(bool enable)
    {
        damagePlayer = enable;
    }

    public void SetProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void SetExplodeRadius(float explodeRadius)
    {
        fireballExplodeRadius = explodeRadius;
    }

    public void SetPlayerCollider(Collider playerCollider)
    {
        this.playerCollider = playerCollider;
    }

    public void SetTimeToLive(float timeToLive)
    {
        this.timeToLive = timeToLive;
    }

    public void EnableCollider(bool enable)
    {
        fireballCollider.enabled = enable;
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
            detonationTime = timeToLive;
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
                if (collider.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine playerStateMachine) && !damagePlayer)
                {
                    continue;
                }
                else
                {
                    health.DealDamage(damage);
                }
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

        if(other.TryGetComponent<Health>(out Health health) || other.gameObject.layer == 6 || other.gameObject.layer == 0)
        {
            ExplodeFireball();
        }

        // if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        // {
        //     forceReceiver.AddForce((other.transform.position - playerCollider.transform.position).normalized * knockback);
        //     Destroy(gameObject);
        // }
    }

    // private void OnDestroy() 
    // {
    //     GameObject explosion = Instantiate(fireballVFX, transform.position, Quaternion.identity);    
    //     Destroy(explosion, 2f);
    // }
}
