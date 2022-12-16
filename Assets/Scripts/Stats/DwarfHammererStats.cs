using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class DwarfHammererStats : MonoBehaviour
    {
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

        [ExecuteInEditMode]
        private void Awake() 
        {
            RefreshStatDisplays();
            EnemyStats.Instance.OnStatsChanged += OnEnemyStatsChanged;
        }

        private void Update() 
        {
            if (!Application.IsPlaying(gameObject))  
            {
                RefreshStatDisplays();
            }  
        }

        public float GetDwarfHammererHealth()
        {
            if (healthOverride == 0)
                return (EnemyStats.Instance.GetEnemyHealth() * healthMultiplicativeModifier) + healthAdditiveModifier;
            else
                return healthOverride;
            
        }

        public float GetDwarfHammererAttack()
        {
            if (attackOverride == 0)
                return (EnemyStats.Instance.GetEnemyAttack() * attackMultiplicativeModifier) + attackAdditiveModifier;
            else
                return attackOverride;
        }

        public float GetDwarfHammererSpeed()
        {
            if (speedOverride == 0)
                return (EnemyStats.Instance.GetEnemySpeed() * speedMultiplicativeModifier) + speedAdditiveModifier;
            else
                return speedOverride;
        }

        private void RefreshStatDisplays()
        {
            Health = GetDwarfHammererHealth();
            Attack = GetDwarfHammererAttack();
            Speed = GetDwarfHammererSpeed();
        }

        private void OnEnemyStatsChanged()
        {
            RefreshStatDisplays();
        }

        private void OnValidate() 
        {
            if (EnemyStats.Instance)
            {
                RefreshStatDisplays();
            }
        }
    }
}

