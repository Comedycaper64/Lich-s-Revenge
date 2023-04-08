using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Miner
{
    public class DwarfMinerChasingState : DwarfMinerBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedParameterHash = Animator.StringToHash("Speed");

        public DwarfMinerChasingState(DwarfMinerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange() || DialogueManager.Instance.inConversation)
            {
                stateMachine.SwitchState(new DwarfMinerIdleState(stateMachine));
                return;
            }
            else if (IsInAttackRange() && stateMachine.debugAttack)
            {
                stateMachine.SwitchState(new DwarfMinerDebugAttackingState(stateMachine));
            }
            else if (IsInAttackRange())
            {
                stateMachine.SwitchState(new DwarfMinerAttackingState(stateMachine));
                return;
            }


            MoveToPlayer(deltaTime);

            FacePlayer();

            stateMachine.Animator.SetFloat(SpeedParameterHash, 1f, 0.1f, deltaTime);
        }

        public override void Exit()
        {
            //Cleans up the NavMesh pathfinding
            if (stateMachine.Agent.hasPath)
            {
                stateMachine.Agent.ResetPath();
            }
            stateMachine.Agent.velocity = Vector3.zero;
            stateMachine.Controller.Move(Vector3.zero);
        }

        private bool IsInAttackRange()
        {
            if (stateMachine.Player.isDead) {return false;}
            //SqrMagnitude is stored due to being more performant than calculating magnitude
            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

            return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
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
