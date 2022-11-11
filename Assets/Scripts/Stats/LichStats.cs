using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichStats : MonoBehaviour
{
    [SerializeField] private float unitHealthModifier;
    [SerializeField] private float unitAttackModifier;

    public float GetLichHealth()
    {
        return PlayerStats.Instance.GetPlayerHealth() * unitHealthModifier;
    }

    public float GetLichAttack()
    {
        return PlayerStats.Instance.GetPlayerAttack() * unitAttackModifier;
    }
}
