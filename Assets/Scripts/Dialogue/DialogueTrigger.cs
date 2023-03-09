using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private Conversation standardConversation;
	[SerializeField] private Conversation storyConversation;
	[SerializeField] private Collider triggerCollider;

    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			TriggerDialogue();
		}
    }

    public void TriggerDialogue()
	{
		if (!DialogueManager.Instance.inConversation)
		{
			if (OptionsManager.Instance.IsStoryMode() && storyConversation != null)
			{
				DialogueManager.Instance.StartConversation(storyConversation);
			}
			else if (standardConversation != null)
			{
				DialogueManager.Instance.StartConversation(standardConversation);
			}
			triggerCollider.enabled = false;
		}
	}
}
