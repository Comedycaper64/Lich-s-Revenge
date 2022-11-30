using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Ranger
{
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

            if (IsInAttackRange())
            {
                stateMachine.SwitchState(new DwarfRangerAttackingState(stateMachine));
            }

            if (IsInFleeRange())
            {
                stateMachine.SwitchState(new DwarfRangerRunningState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
        }

        private bool IsInAttackRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

            return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
        }
    }
}
