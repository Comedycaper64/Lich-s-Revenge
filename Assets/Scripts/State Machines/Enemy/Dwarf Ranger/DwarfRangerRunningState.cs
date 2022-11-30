using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Ranger
{
    public class DwarfRangerRunningState : DwarfRangerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfRangerRunningState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Exit()
        {
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.velocity = Vector3.zero;
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInFleeRange())
            {
                stateMachine.SwitchState(new DwarfRangerIdleState(stateMachine));
                return;
            }

            MoveAwayFromPlayer(deltaTime);

            FaceAwayFromPlayer();

            stateMachine.Animator.SetFloat(SpeedParameterHash, 1f, 0.1f, deltaTime);
        }

        private void MoveAwayFromPlayer(float deltaTime)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = stateMachine.Player.transform.position;
                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.Stats.GetDwarfRangerSpeed() * -1, deltaTime);
            }
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }
    }
}

