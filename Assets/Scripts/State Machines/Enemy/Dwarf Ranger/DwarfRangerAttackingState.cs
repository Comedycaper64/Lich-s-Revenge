using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Ranger
{
    public class DwarfRangerAttackingState : DwarfRangerBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");

        public DwarfRangerAttackingState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();

            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
        }

        public override void Exit()
        {
            stateMachine.RangerWeapon.StowWeapon();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FacePlayer();

            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new DwarfRangerRunningState(stateMachine));
                return;
            }

            stateMachine.RangerWeapon.SetAimVisual();

            // if (!stateMachine.RangerWeapon.weaponFired)
            // {
                
            // }
        }
    }
}