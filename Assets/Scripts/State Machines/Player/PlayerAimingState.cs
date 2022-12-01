using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerAimingState : PlayerBaseState
    {
        private readonly int AimHash = Animator.StringToHash("Aim");

        public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(AimHash, 0.1f);
            stateMachine.InputReader.FireballEvent += OnFireball;   
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.Aegis.ToggleAegis(true);
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.isAttacking && stateMachine.Cooldowns.IsFireboltReady())
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.FireboltStats.GetFireboltSpellManaCost()))
                {
                    stateMachine.SwitchState(new PlayerFireBoltState(stateMachine));
                    return;
                }
            }

            if (stateMachine.InputReader.isHealing && stateMachine.Bones.GetBones() > 0)
            {
                stateMachine.SwitchState(new PlayerHealingState(stateMachine));
                return;
            }
            
            if (!stateMachine.InputReader.isAiming)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

            FaceLookDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputReader.FireballEvent -= OnFireball;
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.Aegis.ToggleAegis(false);
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
            //Rotates player on Y axis to face where camera is looking
            //Doesn't rotate on X and Z because it looks bad with current model, might work with lich

            Quaternion lookDirection = stateMachine.MainCameraTransform.rotation;
            lookDirection.eulerAngles = new Vector3(0, lookDirection.eulerAngles.y, 0);
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, lookDirection, stateMachine.RotationDamping * deltaTime);
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
        
        private void OnFireball()
        {
            stateMachine.SwitchState(new PlayerFireballAimingState(stateMachine));
        }
    }
}
