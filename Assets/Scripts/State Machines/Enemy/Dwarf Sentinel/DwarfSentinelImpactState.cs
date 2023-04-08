using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Sentinel
{
    public class DwarfSentinelImpactState : DwarfSentinelBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("Impact");

        private float duration;

        public DwarfSentinelImpactState(DwarfSentinelStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, 0.1f);
            if (SoundManager.Instance)
            {
                int randomHurtSound = Random.Range(0, stateMachine.hurtSFXs.Length);
                AudioSource.PlayClipAtPoint(stateMachine.hurtSFXs[randomHurtSound], stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            duration = stateMachine.Stats.GetStunDuration();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            duration -= deltaTime;

            if (duration <= 0f)
            {
                stateMachine.SwitchState(new DwarfSentinelIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            
        }
    }
}
