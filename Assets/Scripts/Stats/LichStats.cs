using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichStats : MonoBehaviour
{
    [Header("Health")]
    [ShowOnly] [SerializeField] private float Health;
    [SerializeField] private float healthMultiplicativeModifier;
    [SerializeField] private float healthAdditiveModifier;
    [SerializeField] private float healthOverride = 0;

    [Header("Attack")]
    [ShowOnly] [SerializeField] private float Attack;
    [SerializeField] private float attackMultiplicativeModifier; 
    [SerializeField] private float attackAdditiveModifier;
    [SerializeField] private float attackOverride = 0;

    [Header("Movement Speed")]
    [ShowOnly] [SerializeField] private float Speed;
    [SerializeField] private float speedMultiplicativeModifier; 
    [SerializeField] private float speedAdditiveModifier;
    [SerializeField] private float speedOverride = 0;

    [Header("Spells")]
    [SerializeField] private float spellCooldown;
    [SerializeField] private float spellProjectileSpeed;

    [Header("Dodging")]
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private float dodgeCooldown;

    public event Action OnStatsChanged;

    [ExecuteInEditMode]
    private void Awake() 
    {
        RefreshStatDisplays();
        PlayerStats.Instance.OnStatsChanged += OnPlayerStatsChanged;
    }

    private void Update() 
    {
        if (!Application.IsPlaying(gameObject))  
        {
            RefreshStatDisplays();
        }  
    }

    public float GetLichHealth()
    {
        if (healthOverride == 0)
            return (PlayerStats.Instance.GetPlayerHealth() * healthMultiplicativeModifier) + healthAdditiveModifier;
        else
            return healthOverride;
    }
    public float GetLichAttack()
    {
        if (attackOverride == 0)
            return (PlayerStats.Instance.GetPlayerAttack() * attackMultiplicativeModifier) + attackAdditiveModifier;
        else
            return attackOverride;
    }

    public float GetLichSpeed()
    {
        if (speedOverride == 0)
            return (PlayerStats.Instance.GetPlayerSpeed() * speedMultiplicativeModifier) + speedAdditiveModifier;
        else
            return speedOverride;
    }

    public float GetLichSpellCooldown()
    {
        return spellCooldown;
    }

    public float GetLichSpellProjectileSpeed()
    {
        return spellProjectileSpeed;
    }

    public float GetLichDodgeDistance()
    {
        return dodgeDistance;
    }
    public float GetLichDodgeDuration()
    {
        return dodgeDuration;
    }
    public float GetLichDodgeCooldown()
    {
        return dodgeCooldown;
    }

    private void OnPlayerStatsChanged()
    {
        RefreshStatDisplays();
    }

    private void RefreshStatDisplays()
    {
        Health = GetLichHealth();
        Attack = GetLichAttack();
        Speed = GetLichSpeed();
    }

    private void OnValidate() 
    {
        if (PlayerStats.Instance)
        {
            RefreshStatDisplays();
            OnStatsChanged?.Invoke();
        }
        
    }
}