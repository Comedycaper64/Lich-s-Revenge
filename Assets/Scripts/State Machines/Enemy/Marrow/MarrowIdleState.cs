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

            if ((stateMachine.Health.GetHealthNormalised() < 0.5f) && stateMachine.Cooldowns.IsWaveReady())
            {
                stateMachine.SwitchState(new MarrowWaveState(stateMachine));
                return;
            }

            if (stateMachine.Cooldowns.IsSummonReady())
            {
                stateMachine.SwitchState(new MarrowSummonState(stateMachine));
                return;
            }

            if (stateMachine.Cooldowns.IsFlamePillarReady())
            {
                stateMachine.SwitchState(new MarrowFlamePillarState(stateMachine));
                return;
            }

            if (stateMachine.Cooldowns.IsFireballReady())
            {
                stateMachine.SwitchState(new MarrowFireballState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            
        }
    }
}
