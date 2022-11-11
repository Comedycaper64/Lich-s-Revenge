using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public float playerHealthModifier;
    public float playerAttackModifier;

    public float GetPlayerHealth()
    {
        return BaseStats.Instance.baseHealth * playerHealthModifier;
    }
    public float GetPlayerAttack()
    {
        return BaseStats.Instance.baseAttack * playerAttackModifier;
    }
}
