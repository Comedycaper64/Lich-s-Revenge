using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFireBoltState : PlayerBaseState
    {
        private readonly int FireboltHash = Animator.StringToHash("Firebolt");
        private float previousFrameTime;

        public PlayerFireBoltState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(FireboltHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }
            previousFrameTime = normalisedTime;

            FaceLookDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.Cooldowns.SetFireboltCooldown();
        }    
    }
}
