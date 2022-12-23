using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerHealingState : PlayerBaseState
    {
        private readonly int HealingHash = Animator.StringToHash("Healing");
        private bool hasHealed = false;
        public PlayerHealingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(HealingHash, 0.1f);
            hasHealed = false;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            if (!stateMachine.InputReader.isHealing && normalisedTime <= 0.7f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            if (normalisedTime > 0.7f && !hasHealed)
            {
                Heal();
            }
        }

        public override void Exit()
        {
            
        }

        private void Heal()
        {   
            stateMachine.Bones.TryUseBones(1);
            stateMachine.Health.Heal(stateMachine.LichStats.GetLichHealAmount());
            hasHealed = true;
        }
    }
}
