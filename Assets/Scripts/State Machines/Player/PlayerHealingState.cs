using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerHealingState : PlayerBaseState
    {
        private readonly int HealingHash = Animator.StringToHash("Healing");
        public PlayerHealingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(HealingHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.Health.Heal(20);
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }

            if (!stateMachine.InputReader.isHealing && normalisedTime <= 0.7f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        public override void Exit()
        {
            
        }
    }
}
