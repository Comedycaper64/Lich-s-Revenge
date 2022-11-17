using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EnemyStats : MonoBehaviour
{
    public static EnemyStats Instance {get; private set;}
    public event Action OnStatsChanged;

    [SerializeField] private BaseStats baseStats;

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

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one EnemyStats! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        baseStats.OnStatsChanged += OnBaseStatsChanged;

        RefreshStatDisplays();
    }

    private void Update() 
    {
        if (!Application.IsPlaying(gameObject))  
        {
            RefreshStatDisplays();
        }  
    }

    public float GetEnemyHealth()
    {
        if (healthOverride == 0)
            return (baseStats.baseHealth * healthMultiplicativeModifier) + healthAdditiveModifier;
        else
            return healthOverride;
    }
    public float GetEnemyAttack()
    {
        if (attackOverride == 0)
            return (baseStats.baseAttack * attackMultiplicativeModifier) + attackAdditiveModifier;
        else
            return attackOverride;
    }

    public float GetEnemySpeed()
    {
        if (speedOverride == 0)
            return (baseStats.baseMovementSpeed * speedMultiplicativeModifier) + speedAdditiveModifier;
        else
            return speedOverride;
    }

    private void RefreshStatDisplays()
    {
        Health = GetEnemyHealth();
        Attack = GetEnemyAttack();
        Speed = GetEnemySpeed();
    }

    private void OnBaseStatsChanged()
    {
        RefreshStatDisplays();
        OnStatsChanged?.Invoke();
    }

    private void OnValidate() 
    {
        RefreshStatDisplays();
    }
}
