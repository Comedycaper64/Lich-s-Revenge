using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerImpactState : PlayerBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("Impact");

        private float duration = 0.5f;

        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, 0.1f);
            stateMachine.InputReader.MenuEvent += OnMenu; 
        }

        public override void Exit()
        {
            stateMachine.InputReader.MenuEvent -= OnMenu; 
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            duration -= deltaTime;

            if (duration <= 0f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
        }
    }
}
