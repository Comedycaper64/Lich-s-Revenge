using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerWeapon : MonoBehaviour
{
    private Collider myCollider;
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private float damage;
    private float knockback;
    private float projectileSpeed;

    [SerializeField] private float timeToLive;

    private void Awake() 
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

    public void SetSpeed(float speed)
    {
        projectileSpeed = speed;
    }

    private void OnEnable() 
    {
        alreadyCollidedWith.Clear();    
    }

    public void SetCollider(Collider collider)
    {
        myCollider = collider;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other ==  myCollider) {return;}

        if (alreadyCollidedWith.Contains(other)) {return;}

        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            aegis.DamageAegis(damage, true);
            Destroy(gameObject);
        }

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
            if (!health.isDead && other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                forceReceiver.AddForce((other.transform.position - GetComponent<Collider>().transform.position).normalized * knockback);     
            }
            Destroy(gameObject);
        }

        if (other.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
