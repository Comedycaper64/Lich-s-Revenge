using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerFireballCastState : PlayerBaseState
    {
        private readonly int FireballHash = Animator.StringToHash("Fireball Cast");
        private float previousFrameTime; 

        public PlayerFireballCastState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(FireballHash, 0.1f);
            stateMachine.InputReader.ToggleCameraMovement(false);
            stateMachine.InputReader.DodgeEvent += OnDodge;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
            }
            previousFrameTime = normalisedTime;
        }

        public override void Exit()
        {
            stateMachine.InputReader.ToggleCameraMovement(true);
            stateMachine.Cooldowns.SetFireballCooldown();
            stateMachine.InputReader.DodgeEvent -= OnDodge;
        }

        private void OnDodge()
        {
            if (stateMachine.Cooldowns.IsDodgeReady())
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichDodgeManaCost()))
                {
                    stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
                }
            }
        }
    }
}
