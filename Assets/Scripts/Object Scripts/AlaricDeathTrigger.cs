using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlaricDeathTrigger : MonoBehaviour
{
    private Health alaricHealth;
    [SerializeField] private Conversation deathStoryConversation;
    [SerializeField] private Conversation deathConversation;

    private void Start() 
    {
        alaricHealth = GetComponent<Health>();
        alaricHealth.OnDie += TriggerDialogue;    
    }

    private void TriggerDialogue()
    {
        if (OptionsManager.Instance.IsStoryMode())
        {
            DialogueManager.Instance.StartConversation(deathStoryConversation);
        }
        else
        {
            DialogueManager.Instance.StartConversation(deathConversation);
        }
    }
}
