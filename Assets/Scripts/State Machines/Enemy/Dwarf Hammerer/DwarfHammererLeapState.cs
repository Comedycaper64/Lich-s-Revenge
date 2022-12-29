using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererLeapState : DwarfHammererBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");

        private float leapStartDistance;
        private Vector3 playerPosition;
        private float startYValue;
        private Vector3 movementDirection;

        public DwarfHammererLeapState(DwarfHammererStateMachine stateMachine, float distanceFromPlayer) : base(stateMachine)
        {
            leapStartDistance = distanceFromPlayer;
        }

        public override void Enter()
        {
            FacePlayer();
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.1f);
            playerPosition = stateMachine.Player.transform.position;
            startYValue = stateMachine.transform.position.y;
            movementDirection = playerPosition - stateMachine.transform.position;
        }

        public override void Tick(float deltaTime)
        {
            LeapMove(movementDirection.normalized * stateMachine.Stats.GetSpeed() * 3f , deltaTime);
            Vector3 groundedPosition = stateMachine.transform.position;
            groundedPosition.y = startYValue;
            Vector3 targetDirection = playerPosition - groundedPosition;
            float distanceFromTarget = targetDirection.magnitude;
            float leapYValue = startYValue + (stateMachine.SlamJumpHeight * Mathf.Sin(3.142f * (leapStartDistance - distanceFromTarget) / (1f * leapStartDistance)));
            stateMachine.transform.position = new Vector3(
                stateMachine.transform.position.x,
                leapYValue,
                stateMachine.transform.position.z);
            
            //Once reached ground do big slam attack (Slam State)
            float dotProduct = Vector3.Dot(movementDirection, (targetDirection));
            if ((0.3f > distanceFromTarget) || (stateMachine.Controller.isGrounded && distanceFromTarget < (leapStartDistance / 2)) || (dotProduct < 0f))
            {
                stateMachine.SwitchState(new DwarfHammererIdleState(stateMachine));
                //stateMachine.SwitchState(new DwarfHammererImpactState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.ForceReceiver.AddForce(Vector3.down);
            stateMachine.SetSlamCooldown();
        }
    }
}