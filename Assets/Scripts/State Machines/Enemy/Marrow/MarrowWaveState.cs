using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowWaveState : MarrowBaseState
    {
        private readonly int WaveCastHash = Animator.StringToHash("MarrowLongCast");

        public MarrowWaveState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //On entering this state, the boss teleports to the centre of the arena to begin his "Wave" attack
            GameObject dashVFX = GameObject.Instantiate(stateMachine.teleportVFX, stateMachine.transform.position, Quaternion.identity);
            GameObject.Destroy(dashVFX, 3f);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.teleportSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }

            stateMachine.Controller.enabled = false;
            stateMachine.transform.position = stateMachine.arenaCentre;
            stateMachine.castingWave = true;
            stateMachine.Animator.CrossFadeInFixedTime(WaveCastHash, 0.1f);
            stateMachine.Controller.enabled = true;
        }

        public override void Tick(float deltaTime)
        {
            float normalisedTime = GetNormalizedTime(stateMachine.Animator);
            if (normalisedTime >= 1f)
            {
                stateMachine.Cooldowns.SetActionCooldown();
                stateMachine.SwitchState(new MarrowIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.castingWave = false;
        }
    }
}