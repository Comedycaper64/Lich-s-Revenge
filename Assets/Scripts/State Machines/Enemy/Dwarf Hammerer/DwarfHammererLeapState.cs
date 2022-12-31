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
            stateMachine.WeaponHandler.SetSlamVisualLocation(playerPosition);
            stateMachine.WeaponHandler.EnableSlamVisual(true);
        }

        public override void Tick(float deltaTime)
        {
            LeapMove(movementDirection.normalized * stateMachine.Stats.GetLeapSpeed(), deltaTime);
            Vector3 groundedPosition = stateMachine.transform.position;
            groundedPosition.y = startYValue;
            Vector3 targetDirection = playerPosition - groundedPosition;
            float distanceFromTarget = targetDirection.magnitude;
            float leapYValue = startYValue + (stateMachine.Stats.GetSlamJumpHeight() * Mathf.Sin(Mathf.PI * (leapStartDistance - distanceFromTarget) / (leapStartDistance)));
            stateMachine.transform.position = new Vector3(
                stateMachine.transform.position.x,
                leapYValue,
                stateMachine.transform.position.z);
            
            //Once reached ground do big slam attack (Slam State)
            float dotProduct = Vector3.Dot(movementDirection, (targetDirection));
            if ((0.1f > distanceFromTarget) || (stateMachine.Controller.isGrounded && distanceFromTarget < (leapStartDistance / 2)) || (dotProduct < 0f))
            {
                stateMachine.SwitchState(new DwarfHammererSlamState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            //Manages to reset the navmesh somehow
            stateMachine.ForceReceiver.AddForce(Vector3.up);
            stateMachine.WeaponHandler.EnableSlamVisual(false);
            stateMachine.SetSlamCooldown();
        }
    }
}