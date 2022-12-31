using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererSlamState : DwarfHammererBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("Impact");

        public DwarfHammererSlamState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.WeaponHandler.SetAttack(Mathf.RoundToInt(stateMachine.Stats.GetAttack()), Mathf.RoundToInt(stateMachine.Stats.GetAttackKnockback()));
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, 0.1f);
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