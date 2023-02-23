using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowInactiveState : MarrowBaseState
    {
        private readonly int MovementHash = Animator.StringToHash("");

        public MarrowInactiveState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.Animator.CrossFadeInFixedTime(MovementHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (IsInFightRange())
            {
                stateMachine.SwitchState(new MarrowIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.EnemyUI.SetActive(true);
        }

        private bool IsInFightRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
            return toPlayer.magnitude <= stateMachine.Stats.GetCombatStartRange();
        }
    }
}
