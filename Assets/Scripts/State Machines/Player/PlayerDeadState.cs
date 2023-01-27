using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units.Player
{
    public class PlayerDeadState : PlayerBaseState
    {
        private readonly int DeathHash = Animator.StringToHash("Death");

        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(DeathHash, 0.1f);
            stateMachine.menuManager.ToggleDeathUI(true);
            stateMachine.Controller.enabled = false;
            stateMachine.InputReader.MenuEvent += stateMachine.Respawn;
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {
            stateMachine.menuManager.ToggleDeathUI(false);
            stateMachine.Controller.enabled = true;
            stateMachine.InputReader.MenuEvent -= stateMachine.Respawn;
        }
    }
}
