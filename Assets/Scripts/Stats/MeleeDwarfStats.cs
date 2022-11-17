using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDwarfStats : MonoBehaviour
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

    [ExecuteInEditMode]
    private void Awake() 
    {
        RefreshStatDisplays();
        EnemyStats.Instance.OnStatsChanged += OnEnemyStatsChanged;
    }

    private void Update() 
    {
        if (!Application.IsPlaying(gameObject))  
        {
            RefreshStatDisplays();
        }  
    }

    public float GetMeleeDwarfHealth()
    {
        if (healthOverride == 0)
            return (EnemyStats.Instance.GetEnemyHealth() * healthMultiplicativeModifier) + healthAdditiveModifier;
        else
            return healthOverride;
        
    }

    public float GetMeleeDwarfAttack()
    {
        if (attackOverride == 0)
            return (EnemyStats.Instance.GetEnemyAttack() * attackMultiplicativeModifier) + attackAdditiveModifier;
        else
            return attackOverride;
    }

    public float GetMeleeDwarfSpeed()
    {
        if (speedOverride == 0)
            return (EnemyStats.Instance.GetEnemySpeed() * speedMultiplicativeModifier) + speedAdditiveModifier;
        else
            return speedOverride;
    }

    private void RefreshStatDisplays()
    {
        Health = GetMeleeDwarfHealth();
        Attack = GetMeleeDwarfAttack();
        Speed = GetMeleeDwarfSpeed();
    }

    private void OnEnemyStatsChanged()
    {
        RefreshStatDisplays();
    }

    private void OnValidate() 
    {
        if (EnemyStats.Instance)
        {
            RefreshStatDisplays();
        }
    }
}
