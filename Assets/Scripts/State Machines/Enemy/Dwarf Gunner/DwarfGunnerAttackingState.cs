using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerAttackingState : DwarfGunnerBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");

        public DwarfGunnerAttackingState(DwarfGunnerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();

            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            //FacePlayer();
            if (GetNormalizedTime(stateMachine.Animator) <= 0.1f)
            {
                FacePlayer();
            }

            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new DwarfGunnerRunningState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {

        }
    }
}