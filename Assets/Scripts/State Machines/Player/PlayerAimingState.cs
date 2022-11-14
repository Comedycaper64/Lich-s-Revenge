using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    private readonly int AimHash = Animator.StringToHash("Aim");

    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AimHash, 0.1f);
        //Camera moves to over the shoulder view
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.InputReader.isAiming)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.AimingMovementSpeed, deltaTime);

        FaceLookDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        Debug.Log(stateMachine.transform.rotation.eulerAngles);
        stateMachine.transform.rotation = Quaternion.Euler(0, stateMachine.transform.rotation.y, 0);
    }

    private Vector3 CalculateMovement()
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

    private void FaceLookDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, stateMachine.MainCameraTransform.rotation, stateMachine.RotationDamping * deltaTime);
    }

    
}
