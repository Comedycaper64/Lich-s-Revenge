using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        //private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int MovementHash = Animator.StringToHash("Movement");

        private const float AnimatorDampTime = 0.05f;
        private const float CrossFadeDuration = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.InputReader.JumpEvent += OnJump;
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.InputReader.AbsorbEvent += OnAbsorb; 
            stateMachine.InputReader.MenuEvent += OnMenu; 
            stateMachine.InputReader.MineEvent += OnMine; 
            stateMachine.Animator.CrossFadeInFixedTime(MovementHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.Controller.velocity.y < -2f)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.isHealing && (stateMachine.Bones.GetBones() > 0) && stateMachine.playerUI.healUI.isActiveAndEnabled)
            {
                stateMachine.SwitchState(new PlayerHealingState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.isAiming && stateMachine.playerUI.aimUI.isActiveAndEnabled)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }

            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

            if (stateMachine.InputReader.MovementValue.y < 0)
            {
                FaceMovementDirection(-movement, deltaTime);
            }   
            else
            {
                FaceMovementDirection(movement, deltaTime);
            }
        } 

        public override void Exit()
        {
            stateMachine.InputReader.JumpEvent -= OnJump;
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.AbsorbEvent -= OnAbsorb;
            stateMachine.InputReader.MenuEvent -= OnMenu;  
            stateMachine.InputReader.MineEvent -= OnMine;
        }

        public override string GetStateName()
        {
            return "FreeLookState";
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
            if (movement == Vector3.zero) {return;}
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationDamping * deltaTime);
        }

        private void OnJump()
        {
            if (stateMachine.playerUI.jumpUI.isActiveAndEnabled)
            {
                stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
            }
        }

        private void OnMine()
        {
            if (stateMachine.Controller.isGrounded && stateMachine.Cooldowns.IsMineReady() && stateMachine.playerUI.mineUI.isActiveAndEnabled)
            {
                if (stateMachine.Bones.TryUseBones(1))
                {
                    stateMachine.SwitchState(new PlayerSetMineState(stateMachine));
                }
            }
        }

        private void OnDodge()
        {
            if (stateMachine.Cooldowns.IsDodgeReady() && stateMachine.playerUI.dashUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichDodgeManaCost()))
                {
                    stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
                }
            }
        }

        private void OnAbsorb()
        {
            if (stateMachine.Cooldowns.IsAegisReady() && stateMachine.playerUI.absorbUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichAbsorbManaCost()))
                {
                    stateMachine.SwitchState(new PlayerAbsorbState(stateMachine));
                }
            }
        }


    }
}
