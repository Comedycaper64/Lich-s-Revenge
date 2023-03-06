using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Ranger
{
    public class DwarfRangerChaseState : DwarfRangerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfRangerChaseState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
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
            if (!IsInChaseRange())
            {
                stateMachine.SwitchState(new DwarfRangerIdleState(stateMachine));
                return;
            }

            if (IsInAttackRange())
            {
                stateMachine.SwitchState(new DwarfRangerAttackingState(stateMachine));
                return;
            }

            MoveTowardsPlayer(deltaTime);

            FacePlayer();

            stateMachine.Animator.SetFloat(SpeedParameterHash, 1f, 0.1f, deltaTime);
        }

        private void MoveTowardsPlayer(float deltaTime)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = stateMachine.Player.transform.position;
                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.Stats.GetSpeed(), deltaTime);
            }
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }
    }
}

