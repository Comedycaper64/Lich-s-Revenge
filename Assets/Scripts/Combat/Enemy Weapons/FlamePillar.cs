using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that's attached to the flame pillars summoned by the boss
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
        //Chooses a random direction for the pillar to gradually move in
        ChooseMovementDirection();
    }

    private void Update() 
    {
        transform.Translate(new Vector3(movementDirection.x, 0, movementDirection.y) * movementSpeed * Time.deltaTime);
        if (timeToLive > 0f)
        {
            //Disables the collider of the pillar in the last few seconds of its existence, as the visual effect makes it look like it's waning at the end of its lifespan
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

    //Similar to FlameWave
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
