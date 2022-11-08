using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDwarfStats : MonoBehaviour
{
    [SerializeField] private float unitHealthModifier;
    [SerializeField] private float unitAttackModifier;

    public float GetMeleeDwarfHealth()
    {
        return EnemyStats.Instance.GetEnemyHealth() * unitHealthModifier;
    }

    public float GetMeleeDwarfAttack()
    {
        return EnemyStats.Instance.GetEnemyAttack() * unitAttackModifier;
    }

    private void Start() 
    {
        Debug.Log("Dwarf Health: " + GetMeleeDwarfHealth() + " Dwarf Attack: " + GetMeleeDwarfAttack());    
    }

}
