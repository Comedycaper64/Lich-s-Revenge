using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public static EnemyStats Instance {get; private set;}

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one EnemyStats! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public float enemyHealthModifier;
    public float enemyAttackModifier;

    public float GetEnemyHealth()
    {
        return BaseStats.Instance.baseHealth * enemyHealthModifier;
    }
    public float GetEnemyAttack()
    {
        return BaseStats.Instance.baseAttack * enemyAttackModifier;
    }
}
