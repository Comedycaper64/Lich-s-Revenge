using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Units.Player
{
    public class PlayerStateMachine : StateMachine
    {
        //Note: SerializeField is an attribute that allows the variable to be set in the Unity inspector.

        [field: SerializeField] public InputReader InputReader{get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}
        //PlayerMesh is filled with gameobjects that make up the player's 3D model. Gets set inactive when the player is dashing
        [field: SerializeField] public GameObject[] PlayerMesh{get; private set;}
        //Various scripts that make up the player
        [field: SerializeField] public LichStats LichStats{get; private set;}
        [field: SerializeField] public LichAegis Aegis{get; private set;}
        [field: SerializeField] public FireboltStats FireboltStats {get; private set;}
        [field: SerializeField] public FireballStats FireballStats{get; private set;}
        [field: SerializeField] public PlayerCooldowns Cooldowns{get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public Mana Mana {get; private set;}
        [field: SerializeField] public PlayerWeaponHandler WeaponHandler {get; private set;}
        [field: SerializeField] public LichBones Bones {get; private set;}
        // Rotation damping controls the rate at which the player turns
        [field: SerializeField] public float RotationSpeed {get; private set;}
        //Force applied to player that facilitates a jump. Can't be stored in a state, due to them not being components
        [field: SerializeField] public float JumpForce {get; private set;}
        //Objects that require Instantiation. VFX refer to visual effects
        [field: SerializeField] public GameObject lichMine {get; private set;}
        [field: SerializeField] public GameObject dashVFX {get; private set;}
        [field: SerializeField] public GameObject dashVFX2 {get; private set;}
        [field: SerializeField] public GameObject floatVFX {get; private set;}
        [field: SerializeField] public GameObject jumpVFX {get; private set;}
        //Set location for the Float VFX
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
        //Events can be subscribed to by other scripts. They subscribe by assigning a method to be executed if the event is "invoked"
        public event EventHandler<State> OnSwitchState;
        public static event Action OnRespawn;

        void Start()
        {
            //Lots of initial setup during Frame 1
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

            //Example of this class subscribing methods to events in other scripts
            //If the OnPlayerHealthChanged event is invoked in the OptionsManager, then the method AdjustHealth is executed in this script
            OptionsManager.Instance.OnPlayerHealthChanged += AdjustHealth;
            DialogueManager.Instance.OnConversationStart += SwitchToIdle;

            //Initial state that is switched to
            SwitchState(new PlayerFreeLookState(this));
        }

        public override void SwitchState(State newState)
        {
            base.SwitchState(newState);
            OnSwitchState?.Invoke(this, newState);
        }


        //Used by some enemy units to see if they can perform an action. Returns true if the player is on the ground and not dashing
        public bool CanJumpToPlayer()
        {
            return (Controller.isGrounded && !isDashing);
        }

        private void OnEnable() 
        {   
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        //All events are unsubscribed from if the object is destroyed. If this is not done, errors occur
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

        //When the player enters into dialogue, they should not be in any other state than "FreeLook"
        //This method resets all of the variables in the InputReader that might trigger state transitions
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
        }

        //Sets the player at their last set Respawn point when they die, 
        //while also resetting all of the player's resources and invoking an event that prompts other scripts to perform simular cleanup such that the scene is restarted
        public void Respawn()
        {
            if (respawnPoint != null)
            {
                transform.position = respawnPoint.position;
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
