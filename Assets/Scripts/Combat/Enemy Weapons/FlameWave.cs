using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;
using UnityEngine.VFX;

public class FlameWave : MonoBehaviour
{
    private VisualEffect flameEffect;
    private SphereCollider sphereCollider;
    private Collider casterCollider;
    private float flameDamage;
    private float maxWaveSize;
    private float timeToLive;
    private float duration;

    private void Awake()
    {
        flameEffect = GetComponent<VisualEffect>();
        sphereCollider = GetComponent<SphereCollider>();
        timeToLive = flameEffect.GetFloat("Wave Lifetime");
        Destroy(gameObject, timeToLive);
        maxWaveSize = flameEffect.GetFloat("Max Wave Size");
        duration = 0;
    }

    private void Update() 
    {
        duration += Time.deltaTime;
        float durationNormalised = duration / timeToLive; 
        sphereCollider.radius = maxWaveSize * durationNormalised;
    }

    public void SetCasterCollider(Collider collider)
    {
        casterCollider = collider;
    }

    public void SetDamage(float damage)
    {
        flameDamage = damage;
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
            if (health.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
            {
                if (player.CanJumpToPlayer())
                {
                    health.DealDamage(flameDamage);
                }
            }
            else
            {
                health.DealDamage(flameDamage);
            }
        }
    }
}
