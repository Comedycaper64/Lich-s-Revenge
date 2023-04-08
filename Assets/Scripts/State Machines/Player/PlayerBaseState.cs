using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Player
{
   //All player states derive from this state, which is derived from the State class.
   public abstract class PlayerBaseState : State
   {
      //Provides each PlayerState with a reference to the statemachine
      protected PlayerStateMachine stateMachine;

      public PlayerBaseState(PlayerStateMachine stateMachine)
      {
         this.stateMachine = stateMachine;
      }

      protected void Move(float deltaTime)
      {
         Move(Vector3.zero, deltaTime);
      }

      //Default movement method that some states use. Utilises the CharacterController component to do most of the heavy lifting
      //ForceReceiver movement is applied so that gravity influences the player
      protected void Move(Vector3 motion, float deltaTime)
      {
         stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
      }

      protected void MoveNoGravity(Vector3 motion, float deltaTime)
      {
         stateMachine.Controller.Move(motion * deltaTime);
      }

      protected Vector3 CalculateMovement()
      {
         Vector3 forward = stateMachine.MainCameraTransform.forward;
         forward.y = 0f;
         forward.Normalize();

         Vector3 right = stateMachine.MainCameraTransform.right;
         right.y = 0f;
         right.Normalize();

         return forward * stateMachine.InputReader.MovementValue.y +
               right * stateMachine.InputReader.MovementValue.x;
      }

      //Rotates the player character to face the direction that the camera is looking in
      protected void FaceLookDirection(Vector3 movement, float deltaTime)
      {
         //Rotates player on Y axis to face where camera is looking
         //Doesn't rotate on X and Z because it looks bad

         Quaternion lookDirection = stateMachine.MainCameraTransform.rotation;
         lookDirection.eulerAngles = new Vector3(0, lookDirection.eulerAngles.y, 0);
         stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, lookDirection, stateMachine.RotationSpeed * deltaTime);
      }

      protected void OnMenu()
      {
         stateMachine.menuManager.OpenMenu();
      }
   }
}
