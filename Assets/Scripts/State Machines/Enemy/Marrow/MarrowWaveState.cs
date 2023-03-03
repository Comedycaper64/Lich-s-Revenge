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
            //Flame poof at location
            //Turn off character controller
            //Teleport to arena centre
            stateMachine.Animator.CrossFadeInFixedTime(WaveCastHash, 0.1f);
            //Turn on Character controller
        }

        public override void Tick(float deltaTime)
        {
            //Not moving, not looking at player
            //Every second or so a wave should spawn, originating from Marrow
            //Back to idle state when animation finishes (animation should be slowed wayyy down)
        }

        public override void Exit()
        {
            
        }
    }
}