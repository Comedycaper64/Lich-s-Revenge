using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private Vector3 momentum;

        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
            GameObject jumpEffect = GameObject.Instantiate(stateMachine.jumpVFX, stateMachine.transform.position, Quaternion.identity);
            //Removes the Jump VFX from the scene after three seconds
            GameObject.Destroy(jumpEffect, 3f);
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.jumpSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.InputReader.MenuEvent += OnMenu;

            //When jumping in a certain direction, a portion of the player's momentum in stored
            momentum = stateMachine.Controller.velocity / 3;
            momentum.y = 0f;
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            //This momentum is added to the player's movement in the air, allowing for slightly faster speeds
            Move(momentum + (movement * stateMachine.LichStats.GetLichSpeed()), deltaTime);

            FaceMovementDirection(movement, deltaTime);

            if (stateMachine.Controller.velocity.y <= 0f)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.MenuEvent -= OnMenu;
        }

        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            if (movement == Vector3.zero) {return;}
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationSpeed * deltaTime);
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
