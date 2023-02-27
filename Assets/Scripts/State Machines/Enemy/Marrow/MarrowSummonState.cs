using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowSummonState : MarrowBaseState
    {
        private readonly int SummonHash = Animator.StringToHash("MarrowHeavyCast");

        public MarrowSummonState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(SummonHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FacePlayer();

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);

            if (normalisedTime >= 1f)
            {
                stateMachine.Cooldowns.SetActionCooldown();
                stateMachine.SwitchState(new MarrowIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            
        }
    }
}
