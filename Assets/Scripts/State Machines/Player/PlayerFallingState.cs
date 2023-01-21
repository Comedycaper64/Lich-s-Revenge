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
            momentum = stateMachine.Controller.velocity / 3;
            momentum.y = 0f;
            stateMachine.Animator.CrossFadeInFixedTime(FallHash, 0.1f);
            stateMachine.InputReader.MenuEvent += OnMenu; 
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            Move(momentum + (movement * stateMachine.LichStats.GetLichSpeed()), deltaTime);

            FaceMovementDirection(movement, deltaTime);

            if (stateMachine.Controller.isGrounded)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));  
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.MenuEvent -= OnMenu; 
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = stateMachine.MainCameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = stateMachine.MainCameraTransform.right;
            right.y = 0f;
            right.Normalize();

            return forward * stateMachine.InputReader.MovementValue.y +
                right * stateMachine.InputReader.MovementValue.x;
        }

        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationDamping * deltaTime);
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
