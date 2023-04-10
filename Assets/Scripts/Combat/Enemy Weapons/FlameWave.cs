using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;
using UnityEngine.VFX;

//Script attached to the boss's flame waves
public class FlameWave : MonoBehaviour
{
    private VisualEffect flameEffect;
    private SphereCollider sphereCollider;
    private Collider casterCollider;
    private float flameDamage;
    private float maxWaveSize;
    private float timeToLive;
    private float duration;

    //This object is heavily intertwined with its visual effect, so variables stored within the effect are utilised here as well
    private void Awake()
    {
        flameEffect = GetComponent<VisualEffect>();
        sphereCollider = GetComponent<SphereCollider>();
        timeToLive = flameEffect.GetFloat("Wave Lifetime");
        Destroy(gameObject, timeToLive);
        maxWaveSize = flameEffect.GetFloat("Max Wave Size");
        duration = 0;
    }

    //Gradually increases the size of the wave based on how much of the duration has been gone through
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

        //The player will likely only contact a flame wave once, so if it gets blocked then the damage is disabled
        if(other.TryGetComponent<LichAegis>(out LichAegis aegis))
        {
            aegis.DamageAegis();
            flameDamage = 0;
        }

        //The flame wave travels across the ground. Reuses the "CanJumpToPlayer" check to see if flames will damage the player.
        //CanJumpToPlayer returns true only if the player is on the ground and not dashing
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
