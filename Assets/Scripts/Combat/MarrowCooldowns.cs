using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

//Similar to the PlayerCooldowns script, but for the boss enemy
public class MarrowCooldowns : MonoBehaviour
{
    private MarrowStats stats;

    private float actionCooldown;
    private float fireballCooldown;
    private float flamePillarCooldown;
    private float summonCooldown;
    private float waveCooldown;

    private void Awake() 
    {
        stats = GetComponent<MarrowStats>();
        actionCooldown = 0f;
        fireballCooldown = 0f;
        flamePillarCooldown = 0f;
        summonCooldown = 0f;
        waveCooldown = 0f;
    }

    private void Update() 
    {
        if (actionCooldown > 0f)
        {
            actionCooldown -= Time.deltaTime;
        }    
        if (fireballCooldown > 0f)
        {
            fireballCooldown -= Time.deltaTime;
        }
        if (flamePillarCooldown > 0f)
        {
            flamePillarCooldown -= Time.deltaTime;
        }
        if (summonCooldown > 0f)
        {
            summonCooldown -= Time.deltaTime;
        }
        if (waveCooldown > 0f)
        {
            waveCooldown -= Time.deltaTime;
        }
    }

    public bool IsActionReady()
    {
        return !(actionCooldown > 0f); 
    }

    public void SetActionCooldown()
    {
        actionCooldown = stats.GetActionCooldown();
    }

    public bool IsFireballReady()
    {
        return !(fireballCooldown > 0f);
    }

    public void SetFireballCooldown()
    {
        fireballCooldown = stats.GetFireballCooldown();
    }

    public bool IsFlamePillarReady()
    {
        return !(flamePillarCooldown > 0f);
    }

    public void SetFlamePillarCooldown()
    {
        flamePillarCooldown = stats.GetFlamePillarCooldown();
    }

    public bool IsSummonReady()
    {
        return !(summonCooldown > 0f);
    }

    public void SetSummonCooldown()
    {
        summonCooldown = stats.GetSummonCooldown();
    }

    public bool IsWaveReady()
    {
        return !(waveCooldown > 0f);
    }

    public void SetWaveCooldown()
    {
        waveCooldown = stats.GetFlameWaveCooldown();
    }
}
