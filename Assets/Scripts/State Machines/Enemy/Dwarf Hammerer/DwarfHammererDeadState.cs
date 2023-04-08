using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Hammerer
{
    public class DwarfHammererDeadState : DwarfHammererBaseState
    {
        public DwarfHammererDeadState(DwarfHammererStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.WeaponHandler.DisableWeapon();
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.deathSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            stateMachine.ForceReceiver.enabled = false;
            GameObject.Instantiate(stateMachine.Bone, stateMachine.transform.position, Quaternion.identity);
            stateMachine.Ragdoll.ToggleRagdoll(true);
            stateMachine.EnemyWeapon.SetActive(false);
            stateMachine.EnemyUI.SetActive(false);
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
        
    }
}
