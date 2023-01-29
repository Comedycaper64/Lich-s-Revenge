using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Ranger
{
    public class DwarfRangerIdleState : DwarfRangerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfRangerIdleState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
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

            if (IsInFleeRange())
            {
                stateMachine.SwitchState(new DwarfRangerRunningState(stateMachine));
                return;
            }

            if (CanSeePlayer())
            {
                stateMachine.SwitchState(new DwarfRangerAttackingState(stateMachine));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedParameterHash, 0, 0.1f, deltaTime);
        }

        private bool IsInAttackRange()
        {
            if (stateMachine.Player.isDead) {return false;}

            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

            return playerDistanceSqr <= stateMachine.Stats.GetAttackRange() * stateMachine.Stats.GetAttackRange();
        }

        private bool CanSeePlayer()
        {
            RaycastHit hit;
            Vector3 playerDir = ((stateMachine.Player.transform.position + new Vector3(0, 0.9f, 0)) - stateMachine.headLocation.position).normalized; //adding 0.9f to compensate for height
            if(Physics.Raycast(stateMachine.headLocation.position, playerDir, out hit, stateMachine.Stats.GetAttackRange(), stateMachine.playerVisionLayermask))
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
