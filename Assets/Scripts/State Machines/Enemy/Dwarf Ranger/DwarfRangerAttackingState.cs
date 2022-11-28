using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfRangerAttackingState : DwarfRangerBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");

    public DwarfRangerAttackingState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        FacePlayer();

        stateMachine.Weapon.SetAttack(Mathf.RoundToInt(stateMachine.Stats.GetDwarfRangerAttack()), stateMachine.AttackKnockback);

        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator) >= 1)
            stateMachine.SwitchState(new DwarfRangerRunningState(stateMachine));
    }
}
