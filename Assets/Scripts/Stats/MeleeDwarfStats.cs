using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDwarfStats : MonoBehaviour
{
    [ExecuteInEditMode]
    private void Awake() 
    {
        Health = GetMeleeDwarfHealth();
        Attack = GetMeleeDwarfAttack();
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

    private void OnValidate() 
    {
        if (EnemyStats.Instance)
        {
            Health = GetMeleeDwarfHealth();
            Attack = GetMeleeDwarfAttack();
        }
    }
}
