using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [ExecuteAlways]
    public class FireballStats : MonoBehaviour
    {
        private LichStats lichStats;

        [Header("Spell Cooldown")]
        [ShowOnly] [SerializeField] private float SpellCooldown;
        [SerializeField] private float spellCooldownMultiplicativeModifier;
        [SerializeField] private float spellCooldownAdditiveModifier;
        [SerializeField] private float spellCooldownOverride;

        [Header("Spell Projectile Speed")]
        [ShowOnly] [SerializeField] private float SpellProjectileSpeed;
        [SerializeField] private float spellProjectileSpeedMultiplicativeModifier;
        [SerializeField] private float spellProjectileSpeedAdditiveModifier;
        [SerializeField] private float spellProjectileSpeedOverride;

        [Header("Spell Mana Cost")]
        [ShowOnly] [SerializeField] private float SpellManaCost;
        [SerializeField] private float spellManaCostMultiplicativeModifier;
        [SerializeField] private float spellManaCostAdditiveModifier;
        [SerializeField] private float spellManaCostOverride;

        [Header("Spell Attack")]
        [ShowOnly] [SerializeField] private float SpellAttack;
        [SerializeField] private float spellAttackMultiplicativeModifier;
        [SerializeField] private float spellAttackAdditiveModifier;
        [SerializeField] private float spellAttackOverride;

        [Header("Misc")]
        [SerializeField] private float explodeRadius;
        [SerializeField] private float fireballDetonationTime;
        [SerializeField] private float QTEDamageModifier;
        [SerializeField] private float QTEManaModifier;


        private void Awake() 
        {
            RefreshStatDisplays();
            lichStats = GetComponent<LichStats>();
            lichStats.OnStatsChanged += OnLichStatsChanged;
        }

        private void Update() 
        {
            if (!Application.IsPlaying(gameObject))  
            {
                RefreshStatDisplays();
            }  
        }

        public float GetFireballSpellCooldown()
        {
            if (spellCooldownOverride == 0)
                return (lichStats.GetLichSpellCooldown() * spellCooldownMultiplicativeModifier) + spellCooldownAdditiveModifier;
            else
                return spellCooldownOverride;
        }

        public float GetFireballSpellProjectileSpeed()
        {
            if (spellProjectileSpeedOverride == 0)
                return (lichStats.GetLichSpellProjectileSpeed() * spellProjectileSpeedMultiplicativeModifier) + spellProjectileSpeedAdditiveModifier;
            else
                return spellProjectileSpeedOverride;
        }
        
        public float GetFireballSpellManaCost()
        {
            if (spellManaCostOverride == 0)
                return (lichStats.GetLichSpellManaCost() * spellManaCostMultiplicativeModifier) + spellManaCostAdditiveModifier;
            else
                return spellManaCostOverride;
        }

        public float GetFireballExplodeRadius()
        {
            return explodeRadius;
        }

        public float GetfireballDetonationTime()
        {
            return fireballDetonationTime;
        }

        public float GetFireballQTEMana()
        {
            return GetFireballSpellManaCost() * QTEManaModifier;
        }

        public float GetFireballSpellAttack()
        {
            if (spellAttackOverride == 0)
                return (lichStats.GetLichAttack() * spellAttackMultiplicativeModifier) + spellAttackAdditiveModifier;
            else
                return spellAttackOverride;
        }

        public float GetFireballQTEAttack()
        {
            return GetFireballSpellAttack() * QTEDamageModifier;
        }

        private void OnLichStatsChanged()
        {
            RefreshStatDisplays();
        }

        private void RefreshStatDisplays()
        {
            if (lichStats)
            {
                SpellCooldown = GetFireballSpellCooldown();
                SpellProjectileSpeed = GetFireballSpellProjectileSpeed();
                SpellManaCost = GetFireballSpellManaCost();
                if (PlayerStats.Instance)
                {
                    SpellAttack = GetFireballSpellAttack();
                }
            }
        }

        private void OnValidate() 
        {
            RefreshStatDisplays();   
        }
    }
}
