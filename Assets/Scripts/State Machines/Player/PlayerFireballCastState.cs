using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballCastState : PlayerBaseState
{
    private readonly int FireballHash = Animator.StringToHash("Fireball Cast");
    private float previousFrameTime; 

    public PlayerFireballCastState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FireballHash, 0.1f);
        stateMachine.InputReader.ToggleCameraMovement(false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalisedTime = GetNormalizedTime(stateMachine.Animator);
        if (normalisedTime >= 1f)
        {
            stateMachine.SwitchState(new PlayerAimingState(stateMachine));
        }
        previousFrameTime = normalisedTime;
    }

    public override void Exit()
    {
        stateMachine.InputReader.ToggleCameraMovement(true);
    }
}
