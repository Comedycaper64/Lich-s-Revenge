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
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.WeaponHandler.QTEActive = false;
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

            //When the fireball is being cast, a visual is made to show its predicted travel path
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
            
            stateMachine.Cooldowns.SetFireballCooldown();
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            weaponHandler.UpdateFireballVisual(Vector3.zero);
            weaponHandler.UpdateFireballAimLine(null);
        }


        //A red sphere that shows the player the damage radius of the fireball is place where the fireball is likely to land
        //A raycast is used to determine where the fireball will travel. The layermask for  the raycast includes only layers that the fireball can collide with
        private void DrawAimLine()
        {
            RaycastHit hit;
            int layermask1 = 1 << 6;
            int layermask2 = 1 << 0;
            int layermask3 = 1 << 7;
            int aimLayerMask = layermask1 | layermask2 | layermask3;
            //If the raycast doesn't intersect with an object on the designated layers, then there is no visual
            if (Physics.Raycast(weaponHandler.fireballEmitter.transform.position, weaponHandler.GetDirectionToCameraCentre(weaponHandler.fireballEmitter), out hit, 50f, aimLayerMask))
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
