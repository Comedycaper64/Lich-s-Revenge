using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererIdleState : DwarfHammererBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfHammererIdleState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (CanSeePlayer())
            {
                stateMachine.SwitchState(new DwarfHammererChasingState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
        }

        public override void Exit()
        {

        }

        private bool CanSeePlayer()
        {
            RaycastHit hit;
            Vector3 playerDir = ((stateMachine.Player.transform.position + new Vector3(0, 0.9f, 0)) - stateMachine.headLocation.position).normalized; //adding 0.9f to compensate for height
            if(Physics.Raycast(stateMachine.headLocation.position, playerDir, out hit, stateMachine.Stats.GetChaseRange(), stateMachine.playerVisionLayermask))
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
