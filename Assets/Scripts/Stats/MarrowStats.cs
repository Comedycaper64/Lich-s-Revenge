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

        [Header("Fireball")]
        [SerializeField] private float fireballAttackModifier;
        [SerializeField] private float fireballExplodeRadius;
        [SerializeField] private float fireballTimeToLive;
        [SerializeField] private float fireballSpeed;
        [SerializeField] private float fireballCooldown;

        [Header("Enemy Summon")]
        [SerializeField] private int enemySpawnNumber;
        [SerializeField] private float summonCooldown;

        [Header("Flame Pillars")]
        [SerializeField] private float flamePillarAttack;
        [SerializeField] private int flamePillarNumber;
        [SerializeField] private float flamePillarRadius;
        [SerializeField] private float flamePillarMovement;
        [SerializeField] private float flamePillarSpawnArea;
        [SerializeField] private float flamePillarCooldown;
        [SerializeField] private float flamePillarTimeToLive;

        [Header("Flame Waves")]
        [SerializeField] private float flameWaveAttack;
        [SerializeField] private int flameWaveNumber;
        [SerializeField] private float flameWaveInterval;
        [SerializeField] private float flameWaveCooldown;


        [Header("Movement Speed")]
        [ShowOnly] [SerializeField] private float Speed;
        [SerializeField] private float speedMultiplicativeModifier; 
        [SerializeField] private float speedAdditiveModifier;
        [SerializeField] private float speedOverride = 0;
        

        [Header("Misc")]
        [SerializeField] private float actionCooldown;
        [SerializeField] private float combatStartRange;

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

        public float GetActionCooldown()
        {
            return actionCooldown;
        }

        public float GetFireballCooldown()
        {
            return fireballCooldown;
        }

        public float GetFlamePillarAttack()
        {
            return flamePillarAttack * GetAttack();
        }

        public int GetFlamePillarNumber()
        {
            return flamePillarNumber;
        }

        public float GetFlamePillarRadius()
        {
            return flamePillarRadius;
        }

        public float GetFlamePillarMovement()
        {
            return flamePillarMovement;
        }

        public float GetFlamePillarSpawnArea()
        {
            return flamePillarSpawnArea;
        }

        public float GetFlamePillarCooldown()
        {
            return flamePillarCooldown;
        }

        public float GetFlamePillarTimeToLive()
        {
            return flamePillarTimeToLive;
        }

        public float GetFlameWaveAttack()
        {
            return flameWaveAttack * GetAttack();
        }

        public int GetFlameWaveNumber()
        {
            return flameWaveNumber;
        }

        public float GetFlameWaveInterval()
        {
            return flameWaveInterval;
        }

        public float GetSummonCooldown()
        {
            return summonCooldown;
        }

        public float GetFlameWaveCooldown()
        {
            return flameWaveCooldown;
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
