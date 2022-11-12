using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichStats : MonoBehaviour
{
    [ExecuteInEditMode]
    private void Awake() 
    {
        Health = GetLichHealth();
        Attack = GetLichAttack();
    }

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

    private void OnValidate() 
    {
        if (PlayerStats.Instance)
        {
            Health = GetLichHealth();
            Attack = GetLichAttack();
        }
        
    }
}
