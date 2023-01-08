using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Units.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public InputReader InputReader{get; private set;}
        [field: SerializeField] public GameObject PlayerMesh{get; private set;}
        [field: SerializeField] public LichStats LichStats{get; private set;}
        [field: SerializeField] public LichAegis Aegis{get; private set;}
        [field: SerializeField] public FireboltStats FireboltStats {get; private set;}
        [field: SerializeField] public FireballStats FireballStats{get; private set;}
        [field: SerializeField] public PlayerCooldowns Cooldowns{get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public Mana Mana {get; private set;}
        [field: SerializeField] public PlayerWeaponHandler WeaponHandler {get; private set;}
        [field: SerializeField] public LichBones Bones {get; private set;}
        [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
        [field: SerializeField] public float RotationDamping {get; private set;}
        [field: SerializeField] public float JumpForce {get; private set;}
        [field: SerializeField] public GameObject dashVFX {get; private set;}
        [field: SerializeField] public GameObject dashVFX2 {get; private set;}
        [field: SerializeField] public Transform respawnPoint {get; private set;}
        
        public bool isDashing;

        public Transform MainCameraTransform {get; private set;}
        public MenuManager menuManager;
        public event EventHandler<State> OnSwitchState;
        public event Action OnRespawn;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>();
            MainCameraTransform = Camera.main.transform;

            Health.SetMaxHealth(LichStats.GetLichHealth());
            Mana.SetMaxMana(LichStats.GetLichMaxMana());
            Mana.SetManaRegenRate(LichStats.GetLichManaRegen());

            SwitchState(new PlayerFreeLookState(this));
        }

        public override void SwitchState(State newState)
        {
            base.SwitchState(newState);
            OnSwitchState?.Invoke(this, newState);
        }

        public bool CanJumpToPlayer()
        {
            return (Controller.isGrounded && !isDashing);
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
            SwitchState(new PlayerImpactState(this));
        }

        private void HandleDeath()
        {
            SwitchState(new PlayerDeadState(this));
        }

        public void SetRespawnPoint(Transform respawnTransform)
        {
            respawnPoint = respawnTransform;
        }

        public void Respawn()
        {
            if (respawnPoint != null)
            {
                transform.position = respawnPoint.position;
            }
            Health.Heal(LichStats.GetLichHealth());
            Health.isDead = false;
            SwitchState(new PlayerFreeLookState(this));
            OnRespawn?.Invoke();
        }
    }
}
