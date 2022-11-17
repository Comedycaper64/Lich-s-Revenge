using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FireboltStats : MonoBehaviour
{
    private LichStats lichStats;

    [Header("Spell Cooldown")]
    [ShowOnly] [SerializeField] private float SpellCooldown;
    [SerializeField] private float spellCooldownMultiplicativeModifier;
    [SerializeField] private float spellCooldownAdditiveModifier;
    [SerializeField] private float spellCooldownOverride;

    [Header("Spell Projectile Speed")]
    [ShowOnly] [SerializeField] private float SpellProjectileSpeed;
    [SerializeField] private float spellProjectileSpeedMultiplicativeModifier;
    [SerializeField] private float spellProjectileSpeedAdditiveModifier;
    [SerializeField] private float spellProjectileSpeedOverride;

    private void Awake() 
    {
        RefreshStatDisplays();
        lichStats = GetComponent<LichStats>();
        lichStats.OnStatsChanged += OnLichStatsChanged;
    }

    private void Update() 
    {
        if (!Application.IsPlaying(gameObject))  
        {
            RefreshStatDisplays();
        }  
    }

    public float GetFireboltSpellCooldown()
    {
        if (spellCooldownOverride == 0)
            return (lichStats.GetLichSpellCooldown() * spellCooldownMultiplicativeModifier) + spellCooldownAdditiveModifier;
        else
            return spellCooldownOverride;
    }

    public float GetFireboltSpellProjectileSpeed()
    {
        if (spellProjectileSpeedOverride == 0)
            return (lichStats.GetLichSpellProjectileSpeed() * spellProjectileSpeedMultiplicativeModifier) + spellProjectileSpeedAdditiveModifier;
        else
            return spellProjectileSpeedOverride;
    }

    private void OnLichStatsChanged()
    {
        RefreshStatDisplays();
    }

    private void RefreshStatDisplays()
    {
        if (lichStats)
        {
            SpellCooldown = GetFireboltSpellCooldown();
            SpellProjectileSpeed = GetFireboltSpellProjectileSpeed();
        }
    }

    private void OnValidate() 
    {
        RefreshStatDisplays();
    }
}
