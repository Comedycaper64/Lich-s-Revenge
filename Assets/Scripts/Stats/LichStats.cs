using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    //This method is very similar to the "PlayerStats script". Refer to that for explanations regarding parts of the code
    //This script does not create a static instance, as it's attached to the player
    public class LichStats : MonoBehaviour
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
        [SerializeField] private float attackBuffMultiplier;

        [Header("Movement Speed")]
        [ShowOnly] [SerializeField] private float Speed;
        [SerializeField] private float speedMultiplicativeModifier; 
        [SerializeField] private float speedAdditiveModifier;
        [SerializeField] private float speedOverride = 0;
        [SerializeField] private float castingSpeedMultiplicativeModifier;

        [Header("Mana")]
        [SerializeField] private float maxMana;
        [SerializeField] private float manaRegenRate;

        [Header("Spells")]
        [SerializeField] private float spellCooldown;
        [SerializeField] private float spellProjectileSpeed;
        [SerializeField] private float spellManaCost;
        [SerializeField] private int healAmount;

        [Header("Absorb")]
        [SerializeField] private float absorbCooldown;
        [SerializeField] private float absorbDuration;
        [SerializeField] private float absorbBuffDuration;
        [SerializeField] private float absorbManaMultiplier;

        [Header("Dodging")]
        [SerializeField] private float dodgeDistance;
        [SerializeField] private float dodgeDuration;
        [SerializeField] private float dodgeCooldown;
        [SerializeField] private float dodgeManaCost;
        
        [Header("Mine")]
        [SerializeField] private float mineCooldown;
        [SerializeField] private float mineDamageMultiplier;

        [Header("Misc")]
        [SerializeField] private float lichIFrames;
        public bool debugBadAttackSpread = false;


        public event Action OnStatsChanged;

        [ExecuteInEditMode]
        private void Awake() 
        {
            RefreshStatDisplays();
            PlayerStats.Instance.OnStatsChanged += OnPlayerStatsChanged;
        }

        private void Update() 
        {
            if (!Application.IsPlaying(gameObject))  
            {
                RefreshStatDisplays();
            }  
        }

        public float GetLichHealth()
        {
            if (healthOverride == 0)
                return (PlayerStats.Instance.GetPlayerHealth() * healthMultiplicativeModifier) + healthAdditiveModifier;
            else
                return healthOverride;
        }
        public float GetLichAttack()
        {
            if (attackOverride == 0)
                return (PlayerStats.Instance.GetPlayerAttack() * attackMultiplicativeModifier) + attackAdditiveModifier;
            else
                return attackOverride;
        }

        public float GetLichSpeed()
        {
            if (speedOverride == 0)
                return (PlayerStats.Instance.GetPlayerSpeed() * speedMultiplicativeModifier) + speedAdditiveModifier;
            else
                return speedOverride;
        }

        public float GetCastingMovementSpeed()
        {
            return GetLichSpeed() * castingSpeedMultiplicativeModifier;
        }

        public float GetLichMaxMana()
        {
            return maxMana;
        }

        public float GetLichManaRegen()
        {
            return manaRegenRate;
        }

        public float GetLichSpellCooldown()
        {
            return spellCooldown;
        }

        public float GetLichSpellProjectileSpeed()
        {
            return spellProjectileSpeed;
        }

        public float GetLichSpellManaCost()
        {
            return spellManaCost;
        }

        public int GetLichHealAmount()
        {
            return healAmount;
        }

        public float GetLichAbsorbCooldown()
        {
            return absorbCooldown;
        }

        public float GetLichAbsorbDuration()
        {
            return absorbDuration;
        }

        public float GetLichAbsorbBuffDuration()
        {
            return absorbBuffDuration;
        }

        public float GetLichAbsorbManaCost()
        {
            return GetLichSpellManaCost() * absorbManaMultiplier;
        }

        public float GetLichDodgeDistance()
        {
            return dodgeDistance;
        }
        public float GetLichDodgeDuration()
        {
            return dodgeDuration;
        }
        public float GetLichDodgeCooldown()
        {
            return dodgeCooldown;
        }
        public float GetLichDodgeManaCost()
        {
            return dodgeManaCost;
        }
        public float GetMineCooldown()
        {
            return mineCooldown;
        }
        public float GetMineDamage()
        {
            return GetLichAttack() * mineDamageMultiplier;
        }

        public float GetLichIframes()
        {
            return lichIFrames;
        }

        private void OnPlayerStatsChanged()
        {
            RefreshStatDisplays();
        }

        private void RefreshStatDisplays()
        {
            Health = GetLichHealth();
            Attack = GetLichAttack();
            Speed = GetLichSpeed();
        }

        private void OnValidate() 
        {
            if (PlayerStats.Instance != null)
            {
                RefreshStatDisplays();
                OnStatsChanged?.Invoke();
            }
        }

        //These methods are to facilitate a "damage buff" mechanic provided by the LichAegis script
        public void BuffAttack()
        {
            attackOverride = GetLichAttack() * attackBuffMultiplier;
        }

        public void ResetAttack()
        {
            attackOverride = 0;
        }
    }
}
