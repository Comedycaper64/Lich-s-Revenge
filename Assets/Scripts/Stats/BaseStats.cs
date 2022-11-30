using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
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
