using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.AI;
using Units.Enemy.Miner;
using Units.Player;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public DwarfHammererStats Stats {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public NavMeshAgent Agent {get; private set;}
        [field: SerializeField] public HammererWeaponHandler WeaponHandler {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public GameObject Bone  {get; private set;}
        [field: SerializeField] public GameObject EnemyUI  {get; private set;}
        [field: SerializeField] public GameObject EnemyWeapon  {get; private set;}
        [field: SerializeField] public Ragdoll Ragdoll  {get; private set;}
        [SerializeField] public AudioClip[] hurtSFXs;
        [SerializeField] public AudioClip deathSFX;
        [SerializeField] public AudioClip leapSFX;
        [SerializeField] public AudioClip slamSFX;
        private float slamCooldown = 5f;
        
        public Transform headLocation;
        public int playerVisionLayermask;
        public Health Player {get; private set;}
        public PlayerStateMachine PlayerStateMachine;

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();

            Agent.updatePosition = false;
            Agent.updateRotation = false;

            Health.SetMaxHealth(Stats.GetHealth());
            WeaponHandler.SetupSlamVisual(Stats.GetSlamRadius());
            int layermask1 = 1 << 8;
            int layermask2 = 1 << 6;
            playerVisionLayermask = layermask1 | layermask2;

            SwitchState(new DwarfHammererIdleState(this));    
        }

        public override void Update()
        {
            base.Update();
            if (!IsSlamReady())
            {
                slamCooldown -= Time.deltaTime;
            }
        }

        // public override void SwitchState(State newState)
        // {
        //     base.SwitchState(newState);
        //     Debug.Log(newState);
        // }

        //Have a randomised cooldown running for slam attack
        public bool IsSlamReady()
        {
            return slamCooldown <= 0f;
        }

        public void SetSlamCooldown()
        {
            slamCooldown = Stats.GetSlamCooldown();
        }

        private void OnEnable() 
        {   
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        private void OnDisable() 
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new DwarfHammererImpactState(this));
        }

        private void HandleDeath()
        {
            EnemyUnitDied(gameObject);
            SwitchState(new DwarfHammererDeadState(this));
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.GetChaseRange());    
            //Gizmos.DrawCube(transform.position, new Vector3(Stats.GetSlamRadius() * 2, 1f, Stats.GetSlamRadius() * 2));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Stats.GetSlamRadius());
        }
    }
}
