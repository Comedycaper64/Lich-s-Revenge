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

        protected void SetWaypoint(Transform waypoint)
        {
            stateMachine.currentWaypoint = waypoint.position;
        }

        //Moves between waypoints, chooses a new random waypoint when destination is reached
        protected void Move(float deltaTime)
        {
            if (Mathf.Abs((stateMachine.currentWaypoint - stateMachine.transform.position).magnitude) < 1f)
            {
                SetWaypoint(stateMachine.movementWaypoints[Random.Range(0, stateMachine.movementWaypoints.Length)]);
            }
            Vector3 moveDirection = (stateMachine.currentWaypoint - stateMachine.transform.position).normalized;
            stateMachine.Controller.Move(moveDirection * stateMachine.Stats.GetSpeed() * deltaTime);
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
    }
}
