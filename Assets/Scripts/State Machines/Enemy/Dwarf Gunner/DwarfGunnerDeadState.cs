using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerDeadState : DwarfGunnerBaseState
    {
        public DwarfGunnerDeadState(DwarfGunnerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.enabled = false;
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.deathSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            GameObject.Instantiate(stateMachine.Bone, stateMachine.transform.position, Quaternion.identity);
            stateMachine.Ragdoll.ToggleRagdoll(true);
            stateMachine.EnemyWeapon.SetActive(false);
            stateMachine.EnemyUI.SetActive(false);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}