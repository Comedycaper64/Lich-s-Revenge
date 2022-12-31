using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.AI;
using Units.Enemy.Miner;

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
        private float slamCooldown = 5f;
        

        public Health Player {get; private set;}

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            Agent.updatePosition = false;
            Agent.updateRotation = false;

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetHealth()));
            WeaponHandler.SetupSlamVisual(Stats.GetSlamRadius());

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
            SwitchState(new DwarfHammererDeadState(this));
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.GetChaseRange());    
            Gizmos.DrawCube(transform.position, new Vector3(Stats.GetSlamRadius() * 2, 1f, Stats.GetSlamRadius() * 2));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Stats.GetSlamRadius());
        }
    }
}
