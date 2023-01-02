using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public abstract class DwarfGunnerBaseState : State
    {
        protected DwarfGunnerStateMachine stateMachine;

        public DwarfGunnerBaseState(DwarfGunnerStateMachine stateMachine)
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

        protected void FaceAwayFromPlayer()
        {
            if (stateMachine.Player != null)
            {
                Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
                lookPos.y = 0f;
                stateMachine.transform.rotation = Quaternion.LookRotation(-lookPos);
            }
        }

        protected bool IsInChaseRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

            return playerDistanceSqr <= Mathf.Pow(stateMachine.Stats.GetChaseRange(), 2f);
        }

        protected bool IsInAttackRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

            return playerDistanceSqr <= Mathf.Pow(stateMachine.Stats.GetAttackRange(), 2f);
        }

        protected bool IsInFleeRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            return playerDistanceSqr <= Mathf.Pow(stateMachine.Stats.GetFleeRange(), 2f);
        }
    }
}