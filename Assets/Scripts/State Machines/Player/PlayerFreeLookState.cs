using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Units.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        //Hashes are used to reference specific animations. They are more performant than strings in this context
        private readonly int MovementHash = Animator.StringToHash("Movement");
        //How long it takes to blend between the current animation and the being faded into
        private const float CrossFadeDuration = 0.1f;

        //This constructor is used across all states as a way of having a reference to the statemachine in the new state by storing it in the Base State
        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.InputReader.JumpEvent += OnJump;
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.InputReader.AbsorbEvent += OnAbsorb; 
            stateMachine.InputReader.MenuEvent += OnMenu; 
            stateMachine.InputReader.MineEvent += OnMine; 
            //Scene with build index 10 is the 5th scenario. It includes a mechanic that isn't used in the base game, so is usually not subscribed to
            if (SceneManager.GetActiveScene().buildIndex == 10)
            {
                stateMachine.InputReader.AegisEvent += OnAegis; 
            }
            //Switches to animation specified in the hash over the crossfadeduration
            stateMachine.Animator.CrossFadeInFixedTime(MovementHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.Controller.velocity.y < -2f)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }
            // While most of the code in these if statements is obvious, the use of checking that a specific UI element is enabled is due to
            // a mechanic of the tutorial where you gradually gain your abilities. Basically if the player hasn't been told what an ability does, they should
            // not be able to perform it
            if (stateMachine.InputReader.isHealing && (stateMachine.Bones.GetBones() > 0) && stateMachine.playerUI.healUI.isActiveAndEnabled)
            {
                stateMachine.SwitchState(new PlayerHealingState(stateMachine));
                return;
            }

            if (stateMachine.InputReader.isAiming && stateMachine.playerUI.aimUI.isActiveAndEnabled)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }

            Vector3 movement = CalculateMovement();

            Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

            //The camera exhibits jarring behaviour if the player character is looking towards the camera when the statemachine switches to the AimingState
            //This makes is so that the character always looks forwards, even when moving backwards
            if (stateMachine.InputReader.MovementValue.y < 0)
            {
                FaceMovementDirection(-movement, deltaTime);
            }   
            else
            {
                FaceMovementDirection(movement, deltaTime);
            }
        } 

        public override void Exit()
        {
            stateMachine.InputReader.JumpEvent -= OnJump;
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.AbsorbEvent -= OnAbsorb;
            stateMachine.InputReader.MenuEvent -= OnMenu;  
            stateMachine.InputReader.MineEvent -= OnMine;
            stateMachine.InputReader.AegisEvent -= OnAegis; 
        }

        //A UI script uses this method to see what state the PlayerStateMachine is in.
        public override string GetStateName()
        {
            return "FreeLookState";
        }

        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {   
            if (movement == Vector3.zero) {return;}
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationSpeed * deltaTime);
            
        }

        private void OnAegis()
        {
            if (stateMachine.Cooldowns.IsAegisReady() && stateMachine.playerUI.blockUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichAbsorbManaCost()))
                {
                    stateMachine.SwitchState(new PlayerAegisState(stateMachine));
                }
            }
        }

        private void OnJump()
        {
            if (stateMachine.playerUI.jumpUI.isActiveAndEnabled)
            {
                stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
            }
        }

        private void OnMine()
        {
            if (stateMachine.Controller.isGrounded && stateMachine.Cooldowns.IsMineReady() && stateMachine.playerUI.mineUI.isActiveAndEnabled)
            {
                if (stateMachine.Bones.TryUseBones(1))
                {
                    stateMachine.SwitchState(new PlayerSetMineState(stateMachine));
                }
            }
        }

        private void OnDodge()
        {
            if (stateMachine.Cooldowns.IsDodgeReady() && stateMachine.playerUI.dashUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichDodgeManaCost()))
                {
                    stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
                }
            }
        }

        private void OnAbsorb()
        {
            if (stateMachine.Cooldowns.IsAegisReady() && stateMachine.playerUI.absorbUI.isActiveAndEnabled)
            {
                if (stateMachine.Mana.TryUseMana(stateMachine.LichStats.GetLichAbsorbManaCost()))
                {
                    stateMachine.SwitchState(new PlayerAbsorbState(stateMachine));
                }
            }
        }


    }
}
