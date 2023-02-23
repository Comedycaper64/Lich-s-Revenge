using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public MarrowStats Stats {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public MarrowCooldowns Cooldowns {get; private set;}
        [field: SerializeField] public MarrowWeaponHandler WeaponHandler {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public Ragdoll Ragdoll  {get; private set;}
        [field: SerializeField] public GameObject EnemyUI  {get; private set;}

        [SerializeField] public AudioClip[] hurtSFXs;
        [SerializeField] public AudioClip deathSFX;

        public Health Player {get; private set;}

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetHealth()));

            EnemyStats.Instance.OnHealthChanged += AdjustHealth;

            SwitchState(new MarrowInactiveState(this));    
        }

        private void OnEnable() 
        {   
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        private void AdjustHealth()
        {
            Health.AdjustHealth(Stats.GetHealth());
        }

        private void OnDisable() 
        {
            EnemyStats.Instance.OnHealthChanged -= AdjustHealth;
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new MarrowImpactState(this));
        }

        private void HandleDeath()
        {
            EnemyUnitDied(gameObject);
            SwitchState(new MarrowDeadState(this));
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.GetCombatStartRange());    
        }
    }
}
