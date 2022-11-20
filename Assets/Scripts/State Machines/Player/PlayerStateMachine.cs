using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader{get; private set;}
    [field: SerializeField] public GameObject PlayerMesh{get; private set;}
    [field: SerializeField] public LichStats LichStats{get; private set;}
    [field: SerializeField] public FireboltStats FireboltStats {get; private set;}
    [field: SerializeField] public FireballStats FireballStats{get; private set;}
    [field: SerializeField] public PlayerCooldowns Cooldowns{get; private set;}
    [field: SerializeField] public CharacterController Controller {get; private set;}
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public Targetter Targetter {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public WeaponDamage WeaponDamage {get; private set;}
    [field: SerializeField] public Health Health {get; private set;}
    [field: SerializeField] public Mana Mana {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField] public float RotationDamping {get; private set;}
    [field: SerializeField] public float JumpForce {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    [field: SerializeField] public GameObject dashVFX {get; private set;}

    public Transform MainCameraTransform {get; private set;}

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;

        Health.SetMaxHealth(Mathf.RoundToInt(LichStats.GetLichHealth()));
        Mana.SetMaxMana(LichStats.GetLichMaxMana());
        Mana.SetManaRegenRate(LichStats.GetLichManaRegen());

        SwitchState(new PlayerFreeLookState(this));
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
}
