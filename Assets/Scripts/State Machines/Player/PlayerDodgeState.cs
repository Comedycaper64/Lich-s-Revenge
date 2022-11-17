using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        remainingDodgeTime = stateMachine.Stats.GetLichDodgeDuration();
        stateMachine.Health.SetInvulnerable(true);

        //Fiery poof effect, go invisible, emit flames while moving
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        if (stateMachine.Targetter.CurrentTarget)
        {
            movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.Stats.GetLichDodgeDistance() / stateMachine.Stats.GetLichDodgeDuration();
            movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.Stats.GetLichDodgeDistance() / stateMachine.Stats.GetLichDodgeDuration();
            FaceTarget();
        }
        else
        {
            //Bug: Player flies into the air when dodging if camera is looking up / down
            movement += stateMachine.MainCameraTransform.right * dodgingDirectionInput.x * stateMachine.Stats.GetLichDodgeDistance() / stateMachine.Stats.GetLichDodgeDuration();
            movement += stateMachine.MainCameraTransform.forward * dodgingDirectionInput.y * stateMachine.Stats.GetLichDodgeDistance() / stateMachine.Stats.GetLichDodgeDuration();
        }

        Move(movement, deltaTime);

        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
        stateMachine.SetDodgeCooldown(stateMachine.Stats.GetLichDodgeCooldown());
        //Fiery poof, become visible
    }

 
}
