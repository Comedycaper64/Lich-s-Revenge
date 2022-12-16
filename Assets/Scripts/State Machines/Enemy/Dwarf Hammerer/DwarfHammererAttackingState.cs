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

            stateMachine.WeaponHandler.SetAttack(Mathf.RoundToInt(stateMachine.Stats.GetDwarfHammererAttack()), stateMachine.AttackKnockback);

            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator) >= 1)
                stateMachine.SwitchState(new DwarfHammererChasingState(stateMachine));
        }
    }
}