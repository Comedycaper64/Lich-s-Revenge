using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowInactiveState : MarrowBaseState
    {
        public MarrowInactiveState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            SetWaypoint(stateMachine.movementWaypoints[0]);
        }

        public override void Tick(float deltaTime)
        {
            //Boss does nothing until the player enters the boss's fight range
            if (IsInFightRange())
            {
                stateMachine.SwitchState(new MarrowIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            //Boss UI is activated as fight starts, as does the boss music
            stateMachine.EnemyUI.SetActive(true);
            SoundManager.Instance.PlayAudioSource();
        }

        private bool IsInFightRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
            return toPlayer.magnitude <= stateMachine.Stats.GetCombatStartRange();
        }
    }
}
