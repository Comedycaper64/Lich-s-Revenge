using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Miner
{
    public class DwarfMinerIdleState : DwarfMinerBaseState
    {
        //A blend tree is a list of animations that are "blended" together based on set variables.
        //In this instance "Speed" affects what animation the LocomotionBlendTree is outputing.
        //If speed is zero, then the blend tree outputs an idle animation, if it's 1 then the blend tree outputs a running animation
        //If the speed is somewhere in between 0 and 1 it would be a mix of the idle and running animation
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfMinerIdleState(DwarfMinerStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (DialogueManager.Instance.inConversation) {return;}

            if (CanSeePlayer())
            {
                stateMachine.SwitchState(new DwarfMinerChasingState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
        }

        //Logic for seeing if the player is visible to the enemy using raycasts
        private bool CanSeePlayer()
        {
            RaycastHit hit;
            Vector3 playerDir = ((stateMachine.Player.transform.position + new Vector3(0, 0.9f, 0)) - stateMachine.headLocation.position).normalized; //adding 0.9f to compensate for height
            if(Physics.Raycast(stateMachine.headLocation.position, playerDir, out hit, stateMachine.PlayerChasingRange, stateMachine.playerVisionLayermask))
            {
                // 8 is the layer that the player is on
                if (hit.collider.gameObject.layer == 8)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
