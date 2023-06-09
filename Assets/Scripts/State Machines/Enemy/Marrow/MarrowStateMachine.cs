using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Player;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    //Statemachine for the boss in level 5
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
        //Object containing waypoints that the boss moves between
        [SerializeField] private Transform waypointObject;
        //Object containing locations where enemies can be spawned
        [SerializeField] private Transform enemySpawnObject;
        //Dialogue triggered at the start of the fight (in story mode)
        public Conversation epilogueConversation;
        public Conversation endGameConversation;
        public Transform[] movementWaypoints;
        public Transform[] enemySpawnWaypoints;
        public GameObject[] summonableEnemies;
        public Vector3 currentWaypoint;
        public bool castingWave = false;

        public AudioClip[] hurtSFXs;
        public AudioClip deathSFX;
        public AudioClip teleportSFX;
        public GameObject teleportVFX;
        public Vector3 arenaCentre;

        public Health Player {get; private set;}

        private void Awake() 
        {
            //Populates waypoint lists using objects that contain them
            movementWaypoints = waypointObject.GetComponentsInChildren<Transform>();    
            enemySpawnWaypoints = enemySpawnObject.GetComponentsInChildren<Transform>();
        }

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetHealth()));

            OptionsManager.Instance.OnEnemyHealthChanged += AdjustHealth;

            PlayerStateMachine.OnRespawn += HealToFull;

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
            OptionsManager.Instance.OnEnemyHealthChanged -= AdjustHealth;
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
            PlayerStateMachine.OnRespawn -= HealToFull;
        }

        private void HandleTakeDamage()
        {
            if (castingWave) {return;}
            SwitchState(new MarrowImpactState(this));
        }

        private void HandleDeath()
        {
            EnemyUnitDied(gameObject);
            SwitchState(new MarrowDeadState(this));
        }

        private void HealToFull()
        {
            Health.Heal(999f);
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.GetCombatStartRange());    
        }
    }
}
