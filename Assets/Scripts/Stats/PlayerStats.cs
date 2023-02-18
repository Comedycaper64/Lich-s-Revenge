using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [ExecuteAlways]
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance {get; private set;}
        public event Action OnStatsChanged;
        public event Action OnHealthChanged;

        [SerializeField] private BaseStats baseStats;

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
            DifficultySlider.OnAnyDifficultySliderChanged += OnDifficultyChanged;

            RefreshStatDisplays();
        }

        private void Update() 
        {
            if (!Application.IsPlaying(gameObject))  
            {
                RefreshStatDisplays();
            }  
        }

        public float GetPlayerHealth()
        {
            if (healthOverride == 0)
                return (baseStats.baseHealth * healthMultiplicativeModifier) + healthAdditiveModifier;
            else
                return healthOverride;
        }
        public float GetPlayerAttack()
        {
            if (attackOverride == 0)
                return (baseStats.baseAttack * attackMultiplicativeModifier) + attackAdditiveModifier;
            else
                return attackOverride;
        }

        public float GetPlayerSpeed()
        {
            if (speedOverride == 0)
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

        private void OnDifficultyChanged(object sender, SliderStruct e)
        {
            switch (e.GetDifficultySlider())
            {
                case SliderStruct.DifficultyType.PlayerAttack:
                    attackMultiplicativeModifier = e.GetValue();
                    break;
                case SliderStruct.DifficultyType.PlayerHealth:
                    healthMultiplicativeModifier = e.GetValue();
                    OnHealthChanged?.Invoke();
                    break;
                case SliderStruct.DifficultyType.PlayerSpeed:
                    speedMultiplicativeModifier = e.GetValue();
                    break;
            }
        }

        private void OnValidate() 
        {
            RefreshStatDisplays();
        }
    }
}
