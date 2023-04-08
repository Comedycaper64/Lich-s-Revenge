using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    //A state that calculates a Sin-based jump arc for the enemy
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
            //Initialises variables used for calculating the jump arc in the Tick method
            playerPosition = stateMachine.Player.transform.position;
            startYValue = stateMachine.transform.position.y;
            movementDirection = playerPosition - stateMachine.transform.position;
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.leapSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            //Enables a visual of where damage will be dealt at the location that the enemy is jumping to
            stateMachine.WeaponHandler.SetSlamVisualLocation(playerPosition);
            stateMachine.WeaponHandler.EnableSlamVisual(true);
        }

        public override void Tick(float deltaTime)
        {
            //Moves the enemy closer to the jump destination
            stateMachine.transform.position += (movementDirection.normalized * stateMachine.Stats.GetLeapSpeed() * deltaTime);
            //Calculates how far the enemy is from its destination. It uses its predicted ground position for better accuracy (as for most of the state, the enemy is in the air)
            Vector3 groundedPosition = stateMachine.transform.position;
            groundedPosition.y = startYValue;
            Vector3 targetDirection = playerPosition - groundedPosition;
            float distanceFromTarget = targetDirection.magnitude;
            //A Sin wave is used to calculate how high in the air the enemy should be.
            //By augmenting said Sin wave, the enemy reaches the top of the jump arc at the halfway point of the jump while maintaining a smooth leaping curve
            float leapYValue = startYValue + (stateMachine.Stats.GetSlamJumpHeight() * Mathf.Sin(Mathf.PI * Mathf.Abs((leapStartDistance - distanceFromTarget) / leapStartDistance)));
            //Adjusts enemy's position in accordance to calculation above
            stateMachine.transform.position = new Vector3(
                stateMachine.transform.position.x,
                leapYValue,
                stateMachine.transform.position.z);
 
            //Once reached ground do big slam attack (Slam State)
            //Dot product is used to track if the enemy has reached the destination.
            //In this instance, the Dot product would be negative if the enemy was further than the destination
            float dotProduct = Vector3.Dot(movementDirection.normalized, targetDirection.normalized);
            if ((0.1f > distanceFromTarget) || (dotProduct < 0f))
            {
                stateMachine.SwitchState(new DwarfHammererSlamState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            //Manages to reset the navmesh somehow. If force isn't added to the ForceReceiver then the enemy's pathfinding completely breaks
            stateMachine.ForceReceiver.AddForce(Vector3.up);
            stateMachine.WeaponHandler.EnableSlamVisual(false);
            stateMachine.SetSlamCooldown();
            stateMachine.Controller.enabled = true;
            stateMachine.ForceReceiver.enabled = true;
        }
    }
}