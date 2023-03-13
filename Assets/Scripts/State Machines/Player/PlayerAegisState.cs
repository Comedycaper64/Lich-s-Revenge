using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerAegisState : PlayerBaseState
    {
        private readonly int AbsorbHash = Animator.StringToHash("Absorb");
        private float remainingAbsorbTime;

        public PlayerAegisState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(AbsorbHash, 0.1f);
            stateMachine.Aegis.ToggleAegis(true);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.absorbSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            remainingAbsorbTime = stateMachine.LichStats.GetLichAbsorbDuration();
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);
            FaceLookDirection(movement, deltaTime);
            remainingAbsorbTime -= deltaTime;

            if (remainingAbsorbTime <= 0f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.Aegis.ToggleAegis(false);
            stateMachine.Cooldowns.SetAegisCooldown();
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
