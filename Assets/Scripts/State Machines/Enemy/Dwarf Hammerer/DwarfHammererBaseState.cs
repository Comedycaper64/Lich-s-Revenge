using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public abstract class DwarfHammererBaseState : State
    {
        protected DwarfHammererStateMachine stateMachine;

        public DwarfHammererBaseState(DwarfHammererStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void FacePlayer()
    {
            if (stateMachine.Player != null)
            {
                Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
                lookPos.y = 0f;
                stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
            }
    }

        protected bool IsInChaseRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
            return toPlayer.magnitude <= stateMachine.PlayerChasingRange;
        }
    }
}
