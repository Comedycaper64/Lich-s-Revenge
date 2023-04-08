using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class PlayerSetMineState : PlayerBaseState
{
    //Animations and sound effects from other abilities are re-used here, due to it being a quick action
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
}
