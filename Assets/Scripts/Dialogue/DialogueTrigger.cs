using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private Conversation conversation;
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
			DialogueManager.Instance.StartConversation(conversation);
			triggerCollider.enabled = false;
		}
	}
}
