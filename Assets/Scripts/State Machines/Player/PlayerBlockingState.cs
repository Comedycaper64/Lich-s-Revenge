using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerBlockingState : PlayerBaseState
    {
        private readonly int MovementHash = Animator.StringToHash("Movement");

        private const float AnimatorDampTime = 0.05f;
        private const float CrossFadeDuration = 0.1f;

        public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            // stateMachine.InputReader.DodgeEvent += OnDodge;
            // stateMachine.Animator.CrossFadeInFixedTime(MovementHash, CrossFadeDuration);

            // if (stateMachine.Aegis.canEnable)
            // {
            //     stateMachine.Aegis.ToggleAegis(true);
            // }
            // else if (stateMachine.Cooldowns.IsAegisReady())
            // {
            //     stateMachine.Aegis.ToggleCanEnable(true);
            //     stateMachine.Aegis.ToggleAegis(true);
            // }
        }

        public override void Tick(float deltaTime)
        {
            // if (stateMachine.InputReader.isHealing && stateMachine.Bones.GetBones() > 0)
            // {
            //     stateMachine.SwitchState(new PlayerHealingState(stateMachine));
            //     return;
            // }

            // if (!stateMachine.InputReader.isBlocking)
            // {
            //     stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            //     return;
            // }

            // if (stateMachine.InputReader.isAiming)
            // {
            //     stateMachine.SwitchState(new PlayerAimingState(stateMachine));
            //     return;
            // }

            // Vector3 movement = CalculateMovement();

            // //Move slower while blocking?

            // Move(movement * stateMachine.LichStats.GetCastingMovementSpeed(), deltaTime);

            // // if (stateMachine.InputReader.MovementValue == Vector2.zero)
            // // {
            // //     stateMachine.Animator.SetFloat(MovementHash, 0, AnimatorDampTime, deltaTime);
            // //     return;
            // // }

            // FaceMovementDirection(movement, deltaTime);
        } 

        public override void Exit()
        {
            // stateMachine.InputReader.DodgeEvent -= OnDodge;
            // stateMachine.Aegis.ToggleAegis(false);
        }


        // private Vector3 CalculateMovement()
        // {
        //     Vector3 forward = stateMachine.MainCameraTransform.forward;
        //     forward.y = 0f;
        //     forward.Normalize();

        //     Vector3 right = stateMachine.MainCameraTransform.right;
        //     right.y = 0f;
        //     right.Normalize();

        //     return forward * stateMachine.InputReader.MovementValue.y +
        //         right * stateMachine.InputReader.MovementValue.x;
        // }

        // private void FaceMovementDirection(Vector3 movement, float deltaTime)
        // {
        //     if (movement == Vector3.zero) {return;}

        //     stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationDamping * deltaTime);
        // }


        // private void OnDodge()
        // {
        //     if (stateMachine.Cooldowns.IsDodgeReady())
        //     {
        //         if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichDodgeManaCost()))
        //         {
        //             stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
        //         }
        //     }
        // }
    }
}
