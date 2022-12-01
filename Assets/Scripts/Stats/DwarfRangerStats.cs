using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class DwarfRangerStats : MonoBehaviour
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

        [Header("Other")]
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileTimeToLive;
        [SerializeField] private float attackRange;
        [SerializeField] private float fleeRange;


        [ExecuteInEditMode]
        private void Awake() 
        {
            if (!Application.IsPlaying(gameObject))
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

        public float GetDwarfRangerHealth()
        {
            if (healthOverride == 0)
                return (EnemyStats.Instance.GetEnemyHealth() * healthMultiplicativeModifier) + healthAdditiveModifier;
            else
                return healthOverride;
            
        }

        public float GetDwarfRangerAttack()
        {
            if (attackOverride == 0)
                return (EnemyStats.Instance.GetEnemyAttack() * attackMultiplicativeModifier) + attackAdditiveModifier;
            else
                return attackOverride;
        }

        public float GetDwarfRangerSpeed()
        {
            if (speedOverride == 0)
                return (EnemyStats.Instance.GetEnemySpeed() * speedMultiplicativeModifier) + speedAdditiveModifier;
            else
                return speedOverride;
        }

        public float GetDwarfRangerProjectileSpeed()
        {
            return projectileSpeed;
        }

        public float GetDwarfRangerProjectileTimeToLive()
        {
            return projectileTimeToLive;
        }

        public float GetDwarfRangerAttackRange()
        {
            return attackRange;
        }

        public float GetDwarfRangerFleeRange()
        {
            return fleeRange;
        }

        private void RefreshStatDisplays()
        {
            Health = GetDwarfRangerHealth();
            Attack = GetDwarfRangerAttack();
            Speed = GetDwarfRangerSpeed();
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
