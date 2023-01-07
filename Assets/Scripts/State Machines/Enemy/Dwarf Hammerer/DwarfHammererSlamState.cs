using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererSlamState : DwarfHammererBaseState
    {
        private readonly int SlamHash = Animator.StringToHash("Attack");

        public DwarfHammererSlamState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.WeaponHandler.SetAttack(stateMachine.Stats.GetAttack(), stateMachine.Stats.GetAttackKnockback());
            stateMachine.Animator.CrossFadeInFixedTime(SlamHash, 0.1f);
            stateMachine.WeaponHandler.Slam(stateMachine.Stats.GetSlamRadius());
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new DwarfHammererChasingState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            
        }
    }
}