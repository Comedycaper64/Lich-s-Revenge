using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class MarrowStats : MonoBehaviour
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

        [Header("Spells")]
        [SerializeField] private float fireballAttackModifier;
        [SerializeField] private float fireballExplodeRadius;
        [SerializeField] private float fireballTimeToLive;
        [SerializeField] private float fireballSpeed;
        [SerializeField] private int enemySpawnNumber;

        [Header("Movement Speed")]
        [ShowOnly] [SerializeField] private float Speed;
        [SerializeField] private float speedMultiplicativeModifier; 
        [SerializeField] private float speedAdditiveModifier;
        [SerializeField] private float speedOverride = 0;

        [Header("Cooldowns")]
        [SerializeField] private float actionCooldown;
        [SerializeField] private float fireballCooldown;
        [SerializeField] private float flamePillarCooldown;
        [SerializeField] private float summonCooldown;
        [SerializeField] private float waveCooldown;

        [Header("Misc")]
        [SerializeField] private float combatStartRange;

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

        public float GetActionCooldown()
        {
            return actionCooldown;
        }

        public float GetFireballCooldown()
        {
            return fireballCooldown;
        }

        public float GetFlamePillarCooldown()
        {
            return flamePillarCooldown;
        }

        public float GetSummonCooldown()
        {
            return summonCooldown;
        }

        public float GetWaveCooldown()
        {
            return waveCooldown;
        }

        public float GetCombatStartRange()
        {
            return combatStartRange;
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

        public float GetFireballExplodeRadius()
        {
            return fireballExplodeRadius;
        }

        public float GetFireballAttack()
        {
            return GetAttack() * fireballAttackModifier;
        }

        public float GetFireballDetonationTime()
        {
            return fireballTimeToLive;
        }

        public float GetFireballSpellProjectileSpeed()
        {
            return fireballSpeed;
        }

        public int GetEnemySpawnNumber()
        {
            return enemySpawnNumber;
        }
    }
}
