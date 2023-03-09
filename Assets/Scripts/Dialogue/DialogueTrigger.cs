using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private Conversation standardConversation;
	[SerializeField] private Conversation storyConversation;
	[SerializeField] private Collider triggerCollider;
	[SerializeField] private GameObject inWorldSpeaker;
	[SerializeField] private GameObject inWorldStorySpeaker;

	private void Start() 
	{
		RemoveWorldSpeaker(!OptionsManager.Instance.IsStoryMode());
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			TriggerDialogue();
		}
    }

	private void RemoveWorldSpeaker(bool storySpeaker)
	{
		if (storySpeaker && inWorldStorySpeaker)
		{
			Destroy(inWorldStorySpeaker);
		}
		else if (!storySpeaker && inWorldSpeaker)
		{
			Destroy(inWorldSpeaker);
		}
	}

    public void TriggerDialogue()
	{
		if (!DialogueManager.Instance.inConversation)
		{
			if (OptionsManager.Instance.IsStoryMode() && storyConversation != null)
			{
				RemoveWorldSpeaker(true);
				DialogueManager.Instance.StartConversation(storyConversation);
			}
			else if (standardConversation != null)
			{
				RemoveWorldSpeaker(false);
				DialogueManager.Instance.StartConversation(standardConversation);
			}
			triggerCollider.enabled = false;
		}
	}
}
