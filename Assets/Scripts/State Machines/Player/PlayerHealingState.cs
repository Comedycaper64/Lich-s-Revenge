using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealingState : PlayerBaseState
{
    private readonly int HealingHash = Animator.StringToHash("Healing");
    public PlayerHealingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(HealingHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalisedTime = GetNormalizedTime(stateMachine.Animator);
        if (normalisedTime >= 1f)
        {
            stateMachine.Health.Heal(20);
            ReturnToLocomotion();
        }

        if (!stateMachine.InputReader.isHealing && normalisedTime <= 0.7f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        
    }
}
