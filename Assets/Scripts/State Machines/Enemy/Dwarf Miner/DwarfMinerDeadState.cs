using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Miner
{
    public class DwarfMinerDeadState : DwarfMinerBaseState
    {
        public DwarfMinerDeadState(DwarfMinerStateMachine stateMachine) : base(stateMachine)
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
            //GameObject.Destroy(stateMachine.gameObject);
            stateMachine.EnemyUI.SetActive(false);
            stateMachine.EnemyWeapon.SetActive(false);
            stateMachine.Ragdoll.ToggleRagdoll(true);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}
