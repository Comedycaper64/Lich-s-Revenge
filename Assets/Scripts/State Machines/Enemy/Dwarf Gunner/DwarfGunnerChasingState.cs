using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerChasingState : DwarfGunnerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfGunnerChasingState(DwarfGunnerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Exit()
        {
            if (stateMachine.Agent.hasPath)
            {
                stateMachine.Agent.ResetPath();
            }
            stateMachine.Agent.velocity = Vector3.zero;
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange() || DialogueManager.Instance.inConversation)
            {
                stateMachine.SwitchState(new DwarfGunnerIdleState(stateMachine));
                return;
            }
            else if (IsInAttackRange())
            {
                stateMachine.SwitchState(new DwarfGunnerAttackingState(stateMachine));
                return;
            }


            MoveToPlayer(deltaTime);

            FacePlayer();

            stateMachine.Animator.SetFloat(SpeedParameterHash, 1f, 0.1f, deltaTime);
        }



        private void MoveToPlayer(float deltaTime)
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
