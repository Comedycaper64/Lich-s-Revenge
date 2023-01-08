using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            stateMachine.menuManager.ToggleDeathUI(true);
            stateMachine.InputReader.MenuEvent += stateMachine.Respawn;
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            stateMachine.Ragdoll.ToggleRagdoll(false);
            stateMachine.menuManager.ToggleDeathUI(false);
            stateMachine.InputReader.MenuEvent -= stateMachine.Respawn;
        }
    }
}
