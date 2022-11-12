using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {get; private set;}

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PlayerStats! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Health = GetPlayerHealth();
        Attack = GetPlayerAttack();
    }

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

    public float GetPlayerHealth()
    {
        if (healthOverride == 0)
            return (baseStats.baseHealth * healthMultiplicativeModifier) + healthAdditiveModifier;
        else
            return healthOverride;
    }
    public float GetPlayerAttack()
    {
        if (attackOverride == 0)
            return (baseStats.baseAttack * attackMultiplicativeModifier) + attackAdditiveModifier;
        else
            return attackOverride;
    }

    private void OnValidate() 
    {
        Health = GetPlayerHealth();
        Attack = GetPlayerAttack();
    }
}
