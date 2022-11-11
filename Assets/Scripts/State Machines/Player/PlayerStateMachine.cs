using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader{get; private set;}
    [field: SerializeField] public LichStats Stats{get; private set;}
    [field: SerializeField] public CharacterController Controller {get; private set;}
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public Targetter Targetter {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public WeaponDamage WeaponDamage {get; private set;}
    [field: SerializeField] public Health Health {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField] public float FreeLookMovementSpeed {get; private set;}
    [field: SerializeField] public float TargetingMovementSpeed {get; private set;}
    [field: SerializeField] public float RotationDamping {get; private set;}
    [field: SerializeField] public float DodgeDuration {get; private set;}
    [field: SerializeField] public float DodgeLength {get; private set;}
    [field: SerializeField] public float PreviousDodgeTime {get; private set;} = Mathf.NegativeInfinity;
    [field: SerializeField] public float DodgeCooldown {get; private set;}
    [field: SerializeField] public float JumpForce {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    public Transform MainCameraTransform {get; private set;}

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;

        Health.SetMaxHealth(Mathf.RoundToInt(Stats.GetLichHealth()));

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

    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }
}
