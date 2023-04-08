using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Sentinel
{
    public class DwarfSentinelDeadState : DwarfSentinelBaseState
    {
        public DwarfSentinelDeadState(DwarfSentinelStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.WeaponHandler.DisableWeapon();
            stateMachine.ForceReceiver.enabled = false;
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.deathSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            GameObject.Instantiate(stateMachine.Bone, stateMachine.transform.position, Quaternion.identity);
            stateMachine.EnemyUI.SetActive(false);
            stateMachine.WeaponHandler.EnableSlamVisual(false);
            stateMachine.EnemyWeapon.SetActive(false);
            stateMachine.Ragdoll.ToggleRagdoll(true);
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
    }
}
