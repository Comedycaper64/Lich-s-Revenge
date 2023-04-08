using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererAttackingState : DwarfHammererBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");

        public DwarfHammererAttackingState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();

            stateMachine.WeaponHandler.SetAttack(stateMachine.Stats.GetAttack(), stateMachine.Stats.GetAttackKnockback());

            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
            stateMachine.Animator.speed = stateMachine.Stats.GetAttackSpeed();
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new DwarfHammererChasingState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.Animator.speed = 1;
        }
    }
}