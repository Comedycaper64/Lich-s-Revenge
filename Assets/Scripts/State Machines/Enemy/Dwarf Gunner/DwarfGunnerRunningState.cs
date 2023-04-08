using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerRunningState : DwarfGunnerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfGunnerRunningState(DwarfGunnerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInFleeRange())
            {
                stateMachine.SwitchState(new DwarfGunnerIdleState(stateMachine));
                return;
            }

            MoveAwayFromPlayer(deltaTime);

            FaceAwayFromPlayer();

            stateMachine.Animator.SetFloat(SpeedParameterHash, 1f, 0.1f, deltaTime);
        }

        public override void Exit()
        {
            if (stateMachine.Agent.hasPath)
            {
                stateMachine.Agent.ResetPath();
            }
            stateMachine.Agent.velocity = Vector3.zero;
        }

        private void MoveAwayFromPlayer(float deltaTime)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = stateMachine.Player.transform.position;
                //Calculates where the enemy needs to go to reach the player, then reverses it with a *-1
                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.Stats.GetSpeed() * -1, deltaTime);
            }
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }
    }
}

