using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    //Stats used for player units. Originally intended to be used by more than just the Lich, but that did not come to fruition.
    //This script features a number of techniques to allow seeing variables marked by [ShowOnly] automatically recalculate when a stat is changed.
    [ExecuteAlways]
    public class PlayerStats : MonoBehaviour
    {
        //Static instance of the script is created for ease of getting values from it
        public static PlayerStats Instance {get; private set;}
        public event Action OnStatsChanged;

        [SerializeField] private BaseStats baseStats;

        //[ShowOnly] refers the the ShowOnlyDrawer script, which facilitates having a readonly value in the inspector
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

        private OptionsManager options;

        private void Awake() 
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one PlayerStats! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;

            baseStats.OnStatsChanged += OnBaseStatsChanged;

            options = OptionsManager.Instance;

            RefreshStatDisplays();
        }

        private void Update() 
        {
            if (!Application.IsPlaying(gameObject))  
            {
                RefreshStatDisplays();
            }  
        }

        //A stat is calculated by getting the stat from BaseStats, then multiplying it by the exposed Multiplicative Modifier, 
        //then multiplying it by the difficulty setting option, then adding the additive modifier. This can be overriden to be a particular value using the override.
        public float GetPlayerHealth()
        {
            if ((healthOverride == 0) && options)
                return (baseStats.baseHealth * healthMultiplicativeModifier * options.GetPlayerHealth()) + healthAdditiveModifier;
            else if ((healthOverride == 0))
                return (baseStats.baseHealth * healthMultiplicativeModifier) + healthAdditiveModifier;
            else
                return healthOverride;
        }
        public float GetPlayerAttack()
        {
            if ((attackOverride == 0) && options)
                return (baseStats.baseAttack * attackMultiplicativeModifier * options.GetPlayerAttack()) + attackAdditiveModifier;
            else if (attackOverride == 0)
                return (baseStats.baseAttack * attackMultiplicativeModifier) + attackAdditiveModifier;
            else
                return attackOverride;
        }

        public float GetPlayerSpeed()
        {
            if ((speedOverride == 0) && options)
                return (baseStats.baseMovementSpeed * speedMultiplicativeModifier * options.GetPlayerSpeed()) + speedAdditiveModifier;
            else if (speedOverride == 0)
                return (baseStats.baseMovementSpeed * speedMultiplicativeModifier) + speedAdditiveModifier;
            else
                return speedOverride;
        }

        private void RefreshStatDisplays()
        {
            Health = GetPlayerHealth();
            Attack = GetPlayerAttack();
            Speed = GetPlayerSpeed();
        }

        private void OnBaseStatsChanged()
        {
            RefreshStatDisplays();
            OnStatsChanged?.Invoke();
        }

        //OnValidate is a unity method that is run whenever anything in the inspector is altered
        private void OnValidate() 
        {
            RefreshStatDisplays();
        }
    }
}
