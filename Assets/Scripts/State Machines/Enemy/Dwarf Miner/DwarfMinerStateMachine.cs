using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Enemy.Miner
{
    public class DwarfMinerStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public DwarfMinerStats Stats {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public NavMeshAgent Agent {get; private set;}
        [field: SerializeField] public MinerWeaponHandler WeaponHandler {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public GameObject Bone  {get; private set;}
        [field: SerializeField] public Ragdoll Ragdoll  {get; private set;}
        [field: SerializeField] public GameObject EnemyUI  {get; private set;}
        [field: SerializeField] public GameObject EnemyWeapon  {get; private set;}
        [field: SerializeField] public float PlayerChasingRange {get; private set;}
        [field: SerializeField] public float AttackRange {get; private set;}
        [field: SerializeField] public int AttackKnockback {get; private set;}

        [SerializeField] public AudioClip[] hurtSFXs;
        [SerializeField] public AudioClip deathSFX;

        public Transform headLocation;
        public int playerVisionLayermask;

        public bool debugAttack = false;

        public Health Player {get; private set;}

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            Agent.updatePosition = false;
            Agent.updateRotation = false;

            int layermask1 = 1 << 8;
            int layermask2 = 1 << 6;
            int layermask3 = 1 << 0;
            playerVisionLayermask = layermask1 | layermask2 | layermask3;

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetHealth()));

            OptionsManager.Instance.OnEnemyHealthChanged += AdjustHealth;

            SwitchState(new DwarfMinerIdleState(this));    
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
            OptionsManager.Instance.OnEnemyHealthChanged -= AdjustHealth;
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new DwarfMinerImpactState(this));
        }

        private void HandleDeath()
        {
            EnemyUnitDied(gameObject);
            SwitchState(new DwarfMinerDeadState(this));
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);    
        }
    }
}
