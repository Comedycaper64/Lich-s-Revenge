using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowImpactState : MarrowBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("Impact");

        private float duration = 0.5f;

        public MarrowImpactState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, 0.1f);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.hurtSFXs[0], stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            duration -= deltaTime;

            if (duration <= 0f)
            {
                stateMachine.SwitchState(new MarrowIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            
        }
    }
}
