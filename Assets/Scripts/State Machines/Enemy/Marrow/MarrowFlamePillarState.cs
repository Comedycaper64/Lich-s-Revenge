using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowFlamePillarState : MarrowBaseState
    {
        private readonly int PillarCastHash = Animator.StringToHash("");

        public MarrowFlamePillarState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.Animator.CrossFadeInFixedTime(HeavyCastHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            FacePlayer();

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.SwitchState(new MarrowIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            //Have longer cooldown time after this
            //stateMachine.Cooldowns.SetFlamePillarCooldown();
        }
    }
}