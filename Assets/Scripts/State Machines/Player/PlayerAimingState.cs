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
            stateMachine.Animator.CrossFadeInFixedTime(AimHash, 0.5f);
            stateMachine.InputReader.AbsorbEvent += OnAbsorb; 
            stateMachine.InputReader.DodgeEvent += OnDodge;
        }

        public override void Tick(float deltaTime)
        {
            if (!stateMachine.InputReader.isAiming)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.isAttacking && stateMachine.Cooldowns.IsFireboltReady() && stateMachine.playerUI.fireboltUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.FireboltStats.GetFireboltSpellManaCost()))
                {
                    stateMachine.SwitchState(new PlayerFireBoltState(stateMachine));
                    return;
                }
            }

            if (stateMachine.InputReader.isFireballing && stateMachine.Cooldowns.IsFireballReady() && stateMachine.playerUI.fireballUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.HasMana(stateMachine.FireballStats.GetFireballSpellManaCost())) 
                {
                    stateMachine.SwitchState(new PlayerFireballCastState(stateMachine));
                    return;
                }
            }

            if (stateMachine.InputReader.isHealing && stateMachine.Bones.GetBones() > 0)
            {
                stateMachine.SwitchState(new PlayerHealingState(stateMachine));
                return;
            }
            
            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

            FaceLookDirection(movement, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputReader.AbsorbEvent -= OnAbsorb; 
            stateMachine.InputReader.DodgeEvent -= OnDodge;
        }

        public override string GetStateName()
        {
            return "AimingState";
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
