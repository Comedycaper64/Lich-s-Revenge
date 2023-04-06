using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    //Tier 1 stat script. All attack, health and movement speed values in other stat script are derived from these.
    //An event is used to recalculate displayed values of lower-tier stat scripts in the inspector if these base ones are changed.
    public class BaseStats : MonoBehaviour
    {
        public event Action OnStatsChanged;

        public float baseHealth;
        public float baseAttack;
        public float baseMovementSpeed;

        private void OnValidate() 
        {
            OnStatsChanged?.Invoke();
        }
    }
}
