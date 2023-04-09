using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlaricDeathTrigger : MonoBehaviour
{
    //Script attached to the mini-boss enemy at the end of level 4. The level is completed by defeating him
    private Health alaricHealth;
    [SerializeField] private Conversation deathStoryConversation;
    [SerializeField] private Conversation deathConversation;

    private void Start() 
    {
        alaricHealth = GetComponent<Health>();
        alaricHealth.OnDie += TriggerDialogue;    
    }

    //Dialogue plays on his defeat
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
