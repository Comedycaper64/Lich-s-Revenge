using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererIdleState : DwarfHammererBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfHammererIdleState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
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
                stateMachine.SwitchState(new DwarfHammererChasingState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
        }
    }
}
