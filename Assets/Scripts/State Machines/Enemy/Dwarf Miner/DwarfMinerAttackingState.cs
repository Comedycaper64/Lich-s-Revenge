using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Miner
{
    public class DwarfMinerAttackingState : DwarfMinerBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");

        public DwarfMinerAttackingState(DwarfMinerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();

            stateMachine.WeaponHandler.SetAttack(stateMachine.Stats.GetAttack(), stateMachine.AttackKnockback);

            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
            //There is a difficulty option that influences enemy attack speed. This is achieved by playing the animation at a faster or slower rate
            stateMachine.Animator.speed = stateMachine.Stats.GetAttackSpeed();
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new DwarfMinerChasingState(stateMachine));
                return;
            }
        }
        
        public override void Exit()
        {
            stateMachine.Animator.speed = 1;
        }
    }
}