using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Miner
{
    public class DwarfMinerDebugAttackingState : DwarfMinerBaseState
    {
        //A different animation is played to facilitate a different attack
        private readonly int AttackHash = Animator.StringToHash("DebugAttack");

        public DwarfMinerDebugAttackingState(DwarfMinerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();

            stateMachine.WeaponHandler.SetAttack(stateMachine.Stats.GetAttack(), stateMachine.AttackKnockback);

            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
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
        }
    }
}