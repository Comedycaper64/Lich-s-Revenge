using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFireballCastState : PlayerBaseState
    {
        private readonly int FireballHash = Animator.StringToHash("Fireball");
        private float previousFrameTime; 

        PlayerWeaponHandler weaponHandler;

        public PlayerFireballCastState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            weaponHandler = stateMachine.gameObject.GetComponent<PlayerWeaponHandler>();
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(FireballHash, 0.1f);
            stateMachine.InputReader.ToggleCameraMovement(false);
            stateMachine.InputReader.DodgeEvent += OnDodge;
        }

        public override void Tick(float deltaTime)
        {
            if (!stateMachine.InputReader.isFireballing && stateMachine.WeaponHandler.QTEActive)
            {
                stateMachine.WeaponHandler.CompleteQTE();
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }

            if (!stateMachine.InputReader.isFireballing)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }

            if (normalisedTime <= 0.8f)
            {
                DrawAimLine();
            }

            previousFrameTime = normalisedTime;

            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.LichStats.GetCastingMovementSpeed(), deltaTime);

            FaceLookDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            if (stateMachine.WeaponHandler.fireballLaunched)
            {
                if (stateMachine.WeaponHandler.QTESucceeded)
                {
                    stateMachine.Mana.UseMana(stateMachine.FireballStats.GetFireballQTEMana());
                }
                else
                {
                    stateMachine.Mana.UseMana(stateMachine.FireballStats.GetFireballSpellManaCost());
                }
            }
            
            stateMachine.InputReader.ToggleCameraMovement(true);
            stateMachine.Cooldowns.SetFireballCooldown();
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            weaponHandler.UpdateFireballVisual(Vector3.zero);
            weaponHandler.UpdateFireballAimLine(null);
        }

        private void DrawAimLine()
        {
            RaycastHit hit;
            int layermask = 1 << 6;
            if (Physics.Raycast(weaponHandler.fireballEmitter.transform.position, weaponHandler.GetDirectionToCameraCentre(weaponHandler.fireballEmitter), out hit, 50f, layermask))
            {
                weaponHandler.UpdateFireballVisual(hit.point);
                Vector3[] positionArray = new Vector3[2] {weaponHandler.fireballEmitter.position, hit.point};
                weaponHandler.UpdateFireballAimLine(positionArray); 
            }
            else
            {
                weaponHandler.UpdateFireballVisual(Vector3.zero);
                weaponHandler.UpdateFireballAimLine(null);
            }
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
    }
}
