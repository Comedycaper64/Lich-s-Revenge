using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowFlamePillarState : MarrowBaseState
    {
        private readonly int PillarCastHash = Animator.StringToHash("MarrowStylishCast");

        public MarrowFlamePillarState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(PillarCastHash, 0.1f);
            stateMachine.Animator.speed = stateMachine.Stats.GetAttackSpeed();
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
            stateMachine.WeaponHandler.ClearFlamePillarVisuals();
            stateMachine.Animator.speed = 1;
        }
    }
}