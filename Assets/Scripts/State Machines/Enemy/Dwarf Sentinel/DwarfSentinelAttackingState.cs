using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Sentinel
{
    public class DwarfSentinelAttackingState : DwarfSentinelBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");

        public DwarfSentinelAttackingState(DwarfSentinelStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();
            stateMachine.WeaponHandler.SetAttack(stateMachine.Stats.GetAttack(), stateMachine.AttackKnockback);
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);

            stateMachine.WeaponHandler.SetupSlamVisual(stateMachine.Stats.GetSlamRadius());
            stateMachine.WeaponHandler.SetSlamVisualLocation(stateMachine.transform.position);
            stateMachine.WeaponHandler.SetSlamRadius(stateMachine.Stats.GetSlamRadius());
            stateMachine.WeaponHandler.EnableSlamVisual(true);
            stateMachine.Animator.speed = stateMachine.Stats.GetAttackSpeed();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            //Add event for slam in anim

            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new DwarfSentinelChasingState(stateMachine));
                return;
            }
        }
        
        public override void Exit()
        {
            stateMachine.WeaponHandler.EnableSlamVisual(false);
            stateMachine.Animator.speed = 1;
        }
    }
}