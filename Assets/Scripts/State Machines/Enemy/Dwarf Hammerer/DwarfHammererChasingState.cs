using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererChasingState : DwarfHammererBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfHammererChasingState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            if (!IsInChaseRange(playerDistanceSqr) || DialogueManager.Instance.inConversation)
            {
                stateMachine.SwitchState(new DwarfHammererIdleState(stateMachine));
                return;
            }
            else if (IsInLeapRange(playerDistanceSqr) && stateMachine.IsSlamReady() && stateMachine.PlayerStateMachine.CanJumpToPlayer())
            {
                stateMachine.SwitchState(new DwarfHammererLeapState(stateMachine, Mathf.Sqrt(playerDistanceSqr)));
                return;
            }
            else if (IsInAttackRange(playerDistanceSqr))
            {
                stateMachine.SwitchState(new DwarfHammererAttackingState(stateMachine));
                return;
            }

            MoveToPlayer(deltaTime);

            FacePlayer();

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

        private bool IsInAttackRange(float playerDistanceSqr)
        {
            if (stateMachine.Player.isDead) {return false;}

            return playerDistanceSqr <= Mathf.Pow(stateMachine.Stats.GetAttackRange(), 2f);
        }

        private bool IsInLeapRange(float playerDistanceSqr)
        {
            if (stateMachine.Player.isDead) {return false;}

            return stateMachine.Stats.IsInLeapRange(Mathf.Sqrt(playerDistanceSqr));
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
