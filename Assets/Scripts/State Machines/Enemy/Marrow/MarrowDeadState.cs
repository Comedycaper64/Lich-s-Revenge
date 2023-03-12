using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Marrow
{
    public class MarrowDeadState : MarrowBaseState
    {
        public MarrowDeadState(MarrowStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.EnemyUI.SetActive(false);
            stateMachine.ForceReceiver.enabled = false;
            stateMachine.WeaponHandler.ClearFlamePillars();
            stateMachine.WeaponHandler.StopFlameWaveCoroutine();
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(stateMachine.deathSFX, stateMachine.transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            stateMachine.Ragdoll.ToggleRagdoll(true);
            if (OptionsManager.Instance.IsStoryMode())
			{
				DialogueManager.Instance.StartConversation(stateMachine.epilogueConversation);
			}
            else
            {
                DialogueManager.Instance.StartConversation(stateMachine.endGameConversation);
            }
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
