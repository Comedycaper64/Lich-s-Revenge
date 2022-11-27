using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballAimingState : PlayerBaseState
{
    WeaponHandler weaponHandler;

    public PlayerFireballAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        weaponHandler = stateMachine.gameObject.GetComponent<WeaponHandler>();
    }

    public override void Enter()
    {
        stateMachine.InputReader.FireballEvent += OnFireball;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        //display UI for fireball aim location + aoe
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.isAttacking && stateMachine.Cooldowns.IsFireballReady())
        {
            if (stateMachine.Mana.TryUseMana(stateMachine.FireballStats.GetFireballSpellManaCost())) 
            {
                stateMachine.SwitchState(new PlayerFireballCastState(stateMachine));
                return;
            }
        }

        if (!stateMachine.InputReader.isAiming)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        //Use linerenderer to draw line

        RaycastHit hit;
        int layermask = 1 << 6;
        //layermask = ~layermask;
        if (Physics.Raycast(weaponHandler.fireballEmitter.transform.position, weaponHandler.GetDirectionToCameraCentre(), out hit, 50f, layermask))
        {
            weaponHandler.UpdateFireballVisual(hit.point);
            Vector3[] positionArray = new Vector3[2] {weaponHandler.fireballEmitter.position, hit.point};
            weaponHandler.UpdateFireballAimLine(positionArray); 
        }
        else
        {
            weaponHandler.UpdateFireballVisual(Vector3.zero);
            weaponHandler.UpdateFireballAimLine(null);
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);

        FaceLookDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.FireballEvent -= OnFireball;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        weaponHandler.UpdateFireballVisual(Vector3.zero);
        weaponHandler.UpdateFireballAimLine(null);
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
        //Rotates player on Y axis to face where camera is looking
        //Doesn't rotate on X and Z because it looks bad with current model, might work with lich

        Quaternion lookDirection = stateMachine.MainCameraTransform.rotation;
        lookDirection.eulerAngles = new Vector3(0, lookDirection.eulerAngles.y, 0);
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, lookDirection, stateMachine.RotationDamping * deltaTime);
    }

    private void OnFireball()
    {
        stateMachine.SwitchState(new PlayerAimingState(stateMachine));
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
