using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class PlayerSetMineState : PlayerBaseState
{
    private readonly int SetMineHash = Animator.StringToHash("Absorb");
    private float remainingStateTime;
    public PlayerSetMineState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(SetMineHash, 0.1f);
        remainingStateTime = 0.5f;
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(stateMachine.dashStartSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.LichStats.GetLichSpeed(), deltaTime);
        FaceLookDirection(movement, deltaTime);
        remainingStateTime -= deltaTime;

        if (remainingStateTime <= 0f)
        {
            if (stateMachine.InputReader.isAiming)
            {
                stateMachine.SwitchState(new PlayerAimingState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
        }
    }

    public override void Exit()
    {
        BoneMine boneMine = GameObject.Instantiate(stateMachine.lichMine, stateMachine.transform.position, Quaternion.identity).GetComponent<BoneMine>();
        boneMine.SetDamage(stateMachine.LichStats.GetMineDamage());
        boneMine.SetExplodeRadius(stateMachine.FireballStats.GetFireballExplodeRadius());
        stateMachine.Cooldowns.SetMineCooldown();
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
        Quaternion lookDirection = stateMachine.MainCameraTransform.rotation;
        lookDirection.eulerAngles = new Vector3(0, lookDirection.eulerAngles.y, 0);
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, lookDirection, stateMachine.RotationDamping * deltaTime);
    }

}
