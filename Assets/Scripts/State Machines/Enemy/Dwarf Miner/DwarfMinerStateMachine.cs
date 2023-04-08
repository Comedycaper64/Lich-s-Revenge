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
        //Enemy units have a "ragdoll" that activates upon their death. This causes a loose copy of their body to fall on the floor
        [field: SerializeField] public Ragdoll Ragdoll  {get; private set;}
        //Enemies have a health bar above their heads to indicate how hurt they are. It needs to be disabled upon death, so it's referenced here
        [field: SerializeField] public GameObject EnemyUI  {get; private set;}
        //The object that represents the enemy's weapon. It is also disabled upon death
        [field: SerializeField] public GameObject EnemyWeapon  {get; private set;}
        [field: SerializeField] public float PlayerChasingRange {get; private set;}
        [field: SerializeField] public float AttackRange {get; private set;}
        [field: SerializeField] public int AttackKnockback {get; private set;}

        [SerializeField] public AudioClip[] hurtSFXs;
        [SerializeField] public AudioClip deathSFX;

        public Transform headLocation;
        public int playerVisionLayermask;

        //This enemy has a seperate attack used in one of the scenarios. This bool makes the enemies do that attack instead of their usual
        public bool debugAttack = false;

        //A reference to the player, allowing the enemy to know when the player is within their seight
        public Health Player {get; private set;}

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            //Allowing the NavMeshAgent to affect the enemy's position and rotation causes unintended behaviour
            Agent.updatePosition = false;
            Agent.updateRotation = false;

            //The enemy's vision is blocked by objects on certain layers. This creates the layermask for that purpose
            int layermask1 = 1 << 8;
            int layermask2 = 1 << 6;
            int layermask3 = 1 << 0;
            playerVisionLayermask = layermask1 | layermask2 | layermask3;

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetHealth()));

            //If enemy health is changed in the difficulty settings, the enemy's maxhealth is recalculated
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

        //Enables a visual while editing a scene that shows the enemy's ChaseRange
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);    
        }
    }
}
