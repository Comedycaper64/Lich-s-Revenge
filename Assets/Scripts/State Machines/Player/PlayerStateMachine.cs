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
        //[field: SerializeField] public Transform RigTransform{get; private set;}
        [field: SerializeField] public GameObject[] PlayerMesh{get; private set;}
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
        [field: SerializeField] public float RotationDamping {get; private set;}
        [field: SerializeField] public float JumpForce {get; private set;}
        [field: SerializeField] public GameObject lichMine {get; private set;}
        [field: SerializeField] public GameObject dashVFX {get; private set;}
        [field: SerializeField] public GameObject dashVFX2 {get; private set;}
        [field: SerializeField] public GameObject floatVFX {get; private set;}
        [field: SerializeField] public GameObject jumpVFX {get; private set;}
        [field: SerializeField] public Transform floatPosition {get; private set;}

        [Header("Lich Sound Effects")]
        [SerializeField] public AudioClip jumpSFX;
        [SerializeField] public AudioClip absorbSFX;
        [SerializeField] public AudioClip dashStartSFX;
        [SerializeField] public AudioClip dashEndSFX;
        [SerializeField] public AudioClip deathSFX;


        [SerializeField] private Transform respawnPoint;
        
        public bool isDashing;

        public Transform MainCameraTransform {get; private set;}
        public MenuManager menuManager;
        public PlayerUI playerUI;
        public event EventHandler<State> OnSwitchState;
        public static event Action OnRespawn;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>();
            playerUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>();
            
            Health.healthBar = playerUI.GetHealthBar();
            Mana.manaBar = playerUI.GetManaBar();
            Bones.boneText = playerUI.GetBoneText();

            MainCameraTransform = Camera.main.transform;

            Health.SetMaxHealth(LichStats.GetLichHealth());
            Mana.SetMaxMana(LichStats.GetLichMaxMana());
            Mana.SetManaRegenRate(LichStats.GetLichManaRegen());

            Instantiate(floatVFX, floatPosition);

            OptionsManager.Instance.OnPlayerHealthChanged += AdjustHealth;
            DialogueManager.Instance.OnConversationStart += SwitchToIdle;

            SwitchState(new PlayerFreeLookState(this));
        }

        public override void SwitchState(State newState)
        {
            base.SwitchState(newState);
            //Debug.Log(newState);
            OnSwitchState?.Invoke(this, newState);
        }

        // public override void Update() 
        // {
        //     base.Update();
        //     Debug.Log();
        // }

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
            OptionsManager.Instance.OnPlayerHealthChanged -= AdjustHealth;
            DialogueManager.Instance.OnConversationStart -= SwitchToIdle;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new PlayerImpactState(this));
        }

        private void HandleDeath()
        {
            SwitchState(new PlayerDeadState(this));
        }

        private void AdjustHealth()
        {
            Health.AdjustHealth(LichStats.GetLichHealth());
        }

        private void SwitchToIdle()
        {
            InputReader.OnAim(new UnityEngine.InputSystem.InputAction.CallbackContext());
            InputReader.OnAttack(new UnityEngine.InputSystem.InputAction.CallbackContext());
            InputReader.OnFireball(new UnityEngine.InputSystem.InputAction.CallbackContext());
            InputReader.OnHeal(new UnityEngine.InputSystem.InputAction.CallbackContext());
            SwitchState(new PlayerFreeLookState(this));
        }

        public void SetRespawnPoint(Transform respawnTransform)
        {
            respawnPoint = respawnTransform;
            //Debug.Log("respawn point set:" + respawnPoint.position);
        }

        public void Respawn()
        {
            if (respawnPoint != null)
            {
                //Debug.Log("respawn point:" + respawnPoint.position);
                transform.position = respawnPoint.position;
                //transform.position = transform.TransformPoint(respawnPoint.position);
            }
            Health.Heal(LichStats.GetLichHealth());
            Health.isDead = false;
            Health.SetInvulnerable(false);
            Bones.ResetBones();
            SwitchState(new PlayerFreeLookState(this));
            OnRespawn?.Invoke();
        }
    }
}
