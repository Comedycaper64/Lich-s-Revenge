using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfRangerIdleState : DwarfRangerBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

    public DwarfRangerIdleState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new DwarfRangerRunningState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
    }
}
