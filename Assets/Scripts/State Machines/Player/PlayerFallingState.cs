using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        private readonly int FallHash = Animator.StringToHash("fall");

        private Vector3 momentum;

        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.InputReader.DodgeEvent += OnDodge;
            momentum = stateMachine.Controller.velocity;
            momentum.y = 0f;
            stateMachine.Animator.CrossFadeInFixedTime(FallHash, 0.1f);
            
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum, deltaTime);

            if (stateMachine.Controller.isGrounded)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));  
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.DodgeEvent -= OnDodge;
        }

        private void OnDodge()
        {
            if (stateMachine.Cooldowns.IsDodgeReady())
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichDodgeManaCost()))
                {
                    stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
                }
            }
        }
    }
}
