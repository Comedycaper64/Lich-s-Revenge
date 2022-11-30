using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerDodgeState : PlayerBaseState
    {
        private Vector3 dodgingDirectionInput;
        private float remainingDodgeTime;

        public PlayerDodgeState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
        {
            this.dodgingDirectionInput = dodgingDirectionInput;
        }

        public override void Enter()
        {
            remainingDodgeTime = stateMachine.LichStats.GetLichDodgeDuration();
            stateMachine.Health.SetInvulnerable(true);
            //Fiery poof effect, go invisible, emit flames while moving
            GameObject dashVFX = GameObject.Instantiate(stateMachine.dashVFX, stateMachine.transform.position, Quaternion.identity);
            GameObject.Destroy(dashVFX, 3f);
            stateMachine.PlayerMesh.SetActive(false);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new Vector3();
            //Bug: Player flies into the air when dodging if camera is looking up / down
            movement += stateMachine.MainCameraTransform.right * dodgingDirectionInput.x * stateMachine.LichStats.GetLichDodgeDistance() / stateMachine.LichStats.GetLichDodgeDuration();
            movement += stateMachine.MainCameraTransform.forward * dodgingDirectionInput.y * stateMachine.LichStats.GetLichDodgeDistance() / stateMachine.LichStats.GetLichDodgeDuration();

            Move(movement, deltaTime);

            remainingDodgeTime -= deltaTime;

            if (remainingDodgeTime <= 0f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetInvulnerable(false);
            stateMachine.Cooldowns.SetDodgeCooldown();
            stateMachine.PlayerMesh.SetActive(true);
            GameObject dashVFX = GameObject.Instantiate(stateMachine.dashVFX, stateMachine.transform.position, Quaternion.identity);
            GameObject.Destroy(dashVFX, 3f);
            //Fiery poof, become visible
        } 
    }
}
