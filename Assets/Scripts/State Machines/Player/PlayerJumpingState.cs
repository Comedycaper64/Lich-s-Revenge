using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        //private readonly int JumpHash = Animator.StringToHash("jump");

        private Vector3 momentum;

        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.InputReader.MenuEvent += OnMenu;

            momentum = stateMachine.Controller.velocity;
            momentum.y = 0f;

            //stateMachine.Animator.CrossFadeInFixedTime(JumpHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum, deltaTime);

            if (stateMachine.Controller.velocity.y <= 0f)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }

            //FaceTarget();
        }

        public override void Exit()
        {
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.MenuEvent -= OnMenu;
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
