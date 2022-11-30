using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerDeadState : PlayerBaseState
    {
        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Ragdoll.ToggleRagdoll(true);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}
