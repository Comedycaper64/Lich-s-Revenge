using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

        private const float AnimatorDampTime = 0.05f;
        private const float CrossFadeDuration = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.InputReader.JumpEvent += OnJump;
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }

        public override void Exit()
        {
            stateMachine.InputReader.JumpEvent -= OnJump;
            stateMachine.InputReader.DodgeEvent -= OnDodge;
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.isHealing && stateMachine.Bones.TryUseBones(1))
            {
                stateMachine.SwitchState(new PlayerHealingState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.isAiming)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }

            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
                return;
            }

            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
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

        private void OnJump()
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
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
