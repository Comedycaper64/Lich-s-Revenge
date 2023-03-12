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

        [Header("Hammerer Unique")]
        [SerializeField] private float slamRadius;
        [SerializeField] private float leapSpeedModifier;
        [SerializeField] private float slamCooldownMin;
        [SerializeField] private float slamCooldownMax;
        [SerializeField] private float playerChasingRange; 
        [SerializeField] private float attackRange;
        [SerializeField] private float leapMinRange;
        [SerializeField] private float leapMaxRange; 
        [SerializeField] private int attackKnockback; 
        [SerializeField] private int slamJumpHeight;
        [SerializeField] private float stunDuration;

        [ExecuteInEditMode]
        private void Awake() 
        {
            if (EnemyStats.Instance)
            {
                RefreshStatDisplays();
                EnemyStats.Instance.OnStatsChanged += OnEnemyStatsChanged;
            }
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

        public float GetSlamCooldown()
        {
            return Random.Range(slamCooldownMin, slamCooldownMax);
        }

        public float GetLeapSpeed()
        {
            return GetSpeed() * leapSpeedModifier;
        }

        public float GetSlamRadius()
        {
            return slamRadius;
        }

        public float GetChaseRange()
        {
            return playerChasingRange;
        }

        public float GetAttackRange()
        {
            return attackRange;
        }

        public bool IsInLeapRange(float distanceFromPlayer)
        {
            return (leapMinRange < distanceFromPlayer) && (distanceFromPlayer < leapMaxRange);
        }

        public float GetSlamJumpHeight()
        {
            return slamJumpHeight;
        }

        public float GetAttackKnockback()
        {
            return attackKnockback;
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

