using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFireBoltState : PlayerBaseState
    {
        private readonly int FireboltHash = Animator.StringToHash("Firebolt Cast");
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
            }
            previousFrameTime = normalisedTime;

            FaceLookDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.Cooldowns.SetFireboltCooldown();
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

        private void FaceLookDirection(Vector3 movement, float deltaTime)
        {
            Quaternion lookDirection = stateMachine.MainCameraTransform.rotation;
            lookDirection.eulerAngles = new Vector3(0, lookDirection.eulerAngles.y, 0);
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, lookDirection, stateMachine.RotationDamping * deltaTime);
        }

    
    }
}
