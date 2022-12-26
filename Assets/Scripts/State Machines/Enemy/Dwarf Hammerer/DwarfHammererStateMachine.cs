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
        [field: SerializeField] public float PlayerChasingRange {get; private set;}
        [field: SerializeField] public float AttackRange {get; private set;}
        [field: SerializeField] public int AttackKnockback {get; private set;}

        public Health Player {get; private set;}

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            Agent.updatePosition = false;
            Agent.updateRotation = false;

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetDwarfHammererHealth()));

            SwitchState(new DwarfHammererIdleState(this));    
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
            Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);    
        }
    }
}
