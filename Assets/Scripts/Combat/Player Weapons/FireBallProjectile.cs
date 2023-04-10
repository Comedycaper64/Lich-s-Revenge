using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

//Script attached to the projectile spawned by the player's Fireball ability
public class FireBallProjectile : MonoBehaviour
{
    [SerializeField] private Collider fireballCollider;
    private Collider playerCollider;
    private float damage;
    private float knockback;

    private bool damagePlayer;
    private bool reflected = false;

    private float animationTime = 2f;
    private float timeToLive;
    private float projectileSpeed;
    private bool exploded = false;
    [SerializeField] private GameObject fireballVFX;
    [SerializeField] private GameObject fireballFizzleVFX;
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
            //If flying through the air for long enough, destroys itself
            detonationTime -= Time.deltaTime;
            if (detonationTime <= 0f)
            {
                detonationTime = 0f;
                Destroy(gameObject);
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
    
    //If player's casting of the fireball is interrupted, gets destroyed
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

    //Explodes, dealing damage to enemies in an area
    private void ExplodeFireball()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, fireballExplodeRadius);

        exploded = true;
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
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireballExplodeSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if ((other == playerCollider) && !reflected) {return;}

        if (other.GetComponent<SentinelShield>())
        {
            reflected = true;
            return;
        }
        //When collides with health or layer 6 collider, do damage in sphere with radius based on stats

        if(other.TryGetComponent<Health>(out Health health) || other.gameObject.layer == 6 || other.gameObject.layer == 0)
        {
            ExplodeFireball();
        }
    }

    private void OnDestroy() 
    {
        if (!exploded)
        {
            GameObject fizzle = Instantiate(fireballFizzleVFX, transform.position, Quaternion.identity);    
            Destroy(fizzle, 3f);
        }
    }
}
