using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererLeapState : DwarfHammererBaseState
    {
        private readonly int LeapHash = Animator.StringToHash("Leap");

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
            stateMachine.Controller.enabled = false;
            stateMachine.ForceReceiver.enabled = false;
            FacePlayer();
            stateMachine.Animator.CrossFadeInFixedTime(LeapHash, 0.1f);
            playerPosition = stateMachine.Player.transform.position;
            startYValue = stateMachine.transform.position.y;
            movementDirection = playerPosition - stateMachine.transform.position;
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.leapSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            stateMachine.WeaponHandler.SetSlamVisualLocation(playerPosition);
            stateMachine.WeaponHandler.EnableSlamVisual(true);
        }

        public override void Tick(float deltaTime)
        {
            stateMachine.transform.position += (movementDirection.normalized * stateMachine.Stats.GetLeapSpeed() * deltaTime);
            Vector3 groundedPosition = stateMachine.transform.position;
            groundedPosition.y = startYValue;
            Vector3 targetDirection = playerPosition - groundedPosition;
            float distanceFromTarget = targetDirection.magnitude;
            float leapYValue = startYValue + (stateMachine.Stats.GetSlamJumpHeight() * Mathf.Sin(Mathf.PI * Mathf.Abs((leapStartDistance - distanceFromTarget) / leapStartDistance)));
            //float leapYValue = (stateMachine.Stats.GetSlamJumpHeight() * Mathf.Sin(Mathf.PI * (leapStartDistance - distanceFromTarget) / (leapStartDistance)));
            stateMachine.transform.position = new Vector3(
                stateMachine.transform.position.x,
                leapYValue,
                stateMachine.transform.position.z);
            //Vector3 leapAdd = new Vector3(0, leapYValue, 0);
 
            //Once reached ground do big slam attack (Slam State)
            float dotProduct = Vector3.Dot(movementDirection.normalized, targetDirection.normalized);
            Debug.Log(dotProduct);
            if ((0.1f > distanceFromTarget) || (dotProduct < 0f))
            {
                stateMachine.SwitchState(new DwarfHammererSlamState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            //Manages to reset the navmesh somehow
            //stateMachine.ForceReceiver.AddForce(Vector3.up);
            stateMachine.WeaponHandler.EnableSlamVisual(false);
            stateMachine.SetSlamCooldown();
            stateMachine.Controller.enabled = true;
            stateMachine.ForceReceiver.enabled = true;
        }
    }
}