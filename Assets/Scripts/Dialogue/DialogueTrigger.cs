using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Activates a specified conversation when the player walks into a trigger.
public class DialogueTrigger : MonoBehaviour
{
	//A different conversation is used depending on if the game is in story mode
	[SerializeField] private Conversation standardConversation;
	[SerializeField] private Conversation storyConversation;
	[SerializeField] private Collider triggerCollider;
	//A 2D sprite is used to indicate the presence of a character that initiates a conversation
	[SerializeField] private GameObject inWorldSpeaker;
	[SerializeField] private GameObject inWorldStorySpeaker;

	//No 2D sprites are present if the game is not in story mode
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
