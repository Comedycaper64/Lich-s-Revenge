using System.Collections;
using System.Collections.Generic;
using Units.Enemy.Marrow;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public abstract class MarrowBaseState : State
    {
        protected MarrowStateMachine stateMachine;

        public MarrowBaseState(MarrowStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
            //Move towards waypoint
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move(motion  * deltaTime);
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

        //Method for enumerating between patrols positions from state machine
    }
}
