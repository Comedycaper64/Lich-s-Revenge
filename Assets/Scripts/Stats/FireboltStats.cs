using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    //This method is very similar to the "PlayerStats script". Refer to that for explanations regarding parts of the code
    //This script does not create a static instance, as it's attached to the player
    [ExecuteAlways]
    public class FireboltStats : MonoBehaviour
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

        public float GetFireboltSpellCooldown()
        {
            if (spellCooldownOverride == 0)
                return (lichStats.GetLichSpellCooldown() * spellCooldownMultiplicativeModifier) + spellCooldownAdditiveModifier;
            else
                return spellCooldownOverride;
        }

        public float GetFireboltSpellProjectileSpeed()
        {
            if (spellProjectileSpeedOverride == 0)
                return (lichStats.GetLichSpellProjectileSpeed() * spellProjectileSpeedMultiplicativeModifier) + spellProjectileSpeedAdditiveModifier;
            else
                return spellProjectileSpeedOverride;
        }

        public float GetFireboltSpellManaCost()
        {
            if (spellManaCostOverride == 0)
                return (lichStats.GetLichSpellManaCost() * spellManaCostMultiplicativeModifier) + spellManaCostAdditiveModifier;
            else
                return spellManaCostOverride;
        }

        public float GetFireboltSpellAttack()
        {
            if (spellAttackOverride == 0)
                return (lichStats.GetLichAttack() * spellAttackMultiplicativeModifier) + spellAttackAdditiveModifier;
            else
                return spellAttackOverride;
        }

        private void OnLichStatsChanged()
        {
            RefreshStatDisplays();
        }

        private void RefreshStatDisplays()
        {
            if (lichStats)
            {
                SpellCooldown = GetFireboltSpellCooldown();
                SpellProjectileSpeed = GetFireboltSpellProjectileSpeed();
                SpellManaCost = GetFireboltSpellManaCost();
                if (PlayerStats.Instance)
                {
                    SpellAttack = GetFireboltSpellAttack();
                }
            }
        }

        private void OnValidate() 
        {
            RefreshStatDisplays();
        }
    }
}
