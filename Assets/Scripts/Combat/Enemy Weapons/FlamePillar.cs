using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillar : MonoBehaviour
{
    private Collider casterCollider;
    private Collider pillarCollider;
    private float flameDamage;
    private float movementSpeed;
    private float pillarRadius;
    private float timeToLive;
    private Vector2 movementDirection;

    private void Start() 
    {
        pillarCollider = GetComponent<Collider>();
        ChooseMovementDirection();
    }

    private void Update() 
    {
        transform.Translate(new Vector3(movementDirection.x, 0, movementDirection.y) * movementSpeed * Time.deltaTime);
        if (timeToLive > 0f)
        {
            timeToLive -= Time.deltaTime;
            if ((timeToLive <= 2f) && pillarCollider.enabled)
            {
                pillarCollider.enabled = false;
            }

            if (timeToLive <= 0f)
            {
                timeToLive = 0f;
                Destroy(gameObject);
            }
        }
    }

    private void ChooseMovementDirection()
    {
        movementDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }

    public void SetCasterCollider(Collider collider)
    {
        casterCollider = collider;
    }

    public void SetDamage(float damage)
    {
        flameDamage = damage;
    }

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    public void SetPillarRadius(float radius)
    {
        pillarRadius = radius;
        transform.localScale = new Vector3(radius, 10, radius);
    }

    public void SetTimeToLive(float timeToLive)
    {
        this.timeToLive = timeToLive;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other == casterCollider) {return;}

        if(other.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            aegis.DamageAegis();
            flameDamage = 0;
        }

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(flameDamage);
        }
    }
}
