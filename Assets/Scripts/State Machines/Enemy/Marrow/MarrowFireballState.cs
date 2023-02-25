using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowFireballState : MarrowBaseState
    {
        private readonly int FireballCastHash = Animator.StringToHash("");

        public MarrowFireballState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        //SHOULD BE ABLE TO SHOOT FIREBALL OUT OF HIS HANDS, CAUSING IT TO EXPLODE

        public override void Enter()
        {
            //stateMachine.Animator.CrossFadeInFixedTime(LightCastHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
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
            //stateMachine.Cooldowns.SetFireballCooldown();
        }
    }
}