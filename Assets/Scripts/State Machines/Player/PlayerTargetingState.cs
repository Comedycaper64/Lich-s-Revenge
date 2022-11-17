using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.isAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.Targetter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.Stats.GetLichSpeed(), deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnTarget()
    {
        stateMachine.Targetter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private void OnDodge()
    {
        if (stateMachine.dodgeCooldown <= 0f)
            stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();
        
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        stateMachine.Animator.SetFloat(TargetingForwardHash, Mathf.Round(stateMachine.InputReader.MovementValue.y), 0.1f, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRightHash, Mathf.Round(stateMachine.InputReader.MovementValue.x), 0.1f, deltaTime);
    }
}
