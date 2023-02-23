using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoltProjectile : MonoBehaviour
{
    [SerializeField] private GameObject fireboltExplosion;
    
    private Collider playerCollider;
    private float damage;
    private float knockback;

    private bool reflected = false;

    [SerializeField] private float timeToLive;
    private float projectileSpeed;
    [SerializeField] private AudioClip fireboltExplosionSFX;

    private void Start() 
    {
        Destroy(gameObject, timeToLive);
    }

    private void FixedUpdate() 
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);    
    }

    public void SetAttack(float damage, float knockback)
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

    private void OnTriggerEnter(Collider other) 
    {
        if ((other == playerCollider) && !reflected) {return;}

        if (other.GetComponent<SentinelShield>())
        {
            reflected = true;
            transform.LookAt((playerCollider.transform.position) + new Vector3(0, 0.9f, 0));
            return;
        }

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            forceReceiver.AddForce((other.transform.position - playerCollider.transform.position).normalized * knockback);
            Destroy(gameObject);
        }

        int layer = other.gameObject.layer;
        if ((layer == 6 || layer == 0))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() 
    {
        GameObject explosion = Instantiate(fireboltExplosion, transform.position, Quaternion.identity);  
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireboltExplosionSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }  
        Destroy(explosion, 1f);
    }
}
