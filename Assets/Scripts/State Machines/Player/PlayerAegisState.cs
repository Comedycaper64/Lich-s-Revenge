using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerAegisState : PlayerBaseState
    {
        private readonly int AbsorbHash = Animator.StringToHash("Absorb");
        private float remainingAbsorbTime;

        public PlayerAegisState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(AbsorbHash, 0.1f);
            stateMachine.Aegis.ToggleAegis(true);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.absorbSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            remainingAbsorbTime = stateMachine.LichStats.GetLichAbsorbDuration();
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);
            FaceLookDirection(movement, deltaTime);
            remainingAbsorbTime -= deltaTime;

            if (remainingAbsorbTime <= 0f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.Aegis.ToggleAegis(false);
            stateMachine.Cooldowns.SetAegisCooldown();
        }
    }
}
