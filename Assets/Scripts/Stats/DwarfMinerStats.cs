using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    //This method is very similar to the "PlayerStats script". Refer to that for explanations regarding parts of the code
    //This script does not create a static instance, as it's attached to the enemy that uses it
    public class DwarfMinerStats : MonoBehaviour
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

        [SerializeField] private float stunDuration;

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

        public float GetStunDuration()
        {
            return stunDuration * OptionsManager.Instance.GetEnemyStunTime();
        }

        public float GetAttackSpeed()
        {
            return OptionsManager.Instance.GetEnemyAttackSpeed();
        }

        public float GetHealth()
        {
            if (healthOverride == 0)
                return (EnemyStats.Instance.GetEnemyHealth() * healthMultiplicativeModifier) + healthAdditiveModifier;
            else
                return healthOverride;
            
        }

        public float GetAttack()
        {
            if (attackOverride == 0)
                return (EnemyStats.Instance.GetEnemyAttack() * attackMultiplicativeModifier) + attackAdditiveModifier;
            else
                return attackOverride;
        }

        public float GetSpeed()
        {
            if (speedOverride == 0)
                return (EnemyStats.Instance.GetEnemySpeed() * speedMultiplicativeModifier) + speedAdditiveModifier;
            else
                return speedOverride;
        }

        private void RefreshStatDisplays()
        {
            Health = GetHealth();
            Attack = GetAttack();
            Speed = GetSpeed();
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
