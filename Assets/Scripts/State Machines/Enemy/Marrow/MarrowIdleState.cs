using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowIdleState : MarrowBaseState
    {
        private readonly int MovementHash = Animator.StringToHash("MarrowFloat");

        public MarrowIdleState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(MovementHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FacePlayer();
            
            if (!stateMachine.Cooldowns.IsActionReady()) {return;}

            if (stateMachine.Cooldowns.IsSummonReady())
            {
                stateMachine.SwitchState(new MarrowSummonState(stateMachine));
            }

            if (stateMachine.Cooldowns.IsFireballReady())
            {
                stateMachine.SwitchState(new MarrowFireballState(stateMachine));
                return;
            }

            // Moves around random patrol points in map (point chosing logic in Base state so that all states can use it)
            //Big if-else section about various actions that he can do
        }

        public override void Exit()
        {
            
        }
    }
}
