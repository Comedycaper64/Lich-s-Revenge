using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerIdleState : DwarfGunnerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfGunnerIdleState(DwarfGunnerStateMachine stateMachine) : base(stateMachine)
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
                stateMachine.SwitchState(new DwarfGunnerAttackingState(stateMachine));
                return;
            }

            if (IsInChaseRange())
            {
                stateMachine.SwitchState(new DwarfGunnerChasingState(stateMachine));
                return;
            }

            if (IsInFleeRange())
            {
                stateMachine.SwitchState(new DwarfGunnerRunningState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
        }
    }
}
