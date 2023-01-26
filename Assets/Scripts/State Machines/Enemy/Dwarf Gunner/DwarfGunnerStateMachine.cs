using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public DwarfGunnerStats Stats {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public NavMeshAgent Agent {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public GameObject Bone {get; private set;}
        [field: SerializeField] public GunnerWeaponHandler GunnerWeapon {get; private set;}
        //[field: SerializeField] public int AttackKnockback {get; private set;}

        public Health Player {get; private set;}

        private void Start() 
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            Agent.updatePosition = false;
            Agent.updateRotation = false;

            Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetHealth()));
            SwitchState(new DwarfGunnerIdleState(this));    
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
            SwitchState(new DwarfGunnerImpactState(this));
        }

        private void HandleDeath()
        {
            EnemyUnitDied(gameObject);
            SwitchState(new DwarfGunnerDeadState(this));
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Stats.GetAttackRange());   
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Stats.GetChaseRange());   
        }
    }
}