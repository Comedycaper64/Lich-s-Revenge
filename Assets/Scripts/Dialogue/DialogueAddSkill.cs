using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used in a conversation to trigger the activation of a skill in the PlayerUI
[CreateAssetMenu(menuName = "DialogueAddSkill", fileName = "NewDialogueAddItem")]
public class DialogueAddSkill : ConversationNode
{
    public LichSkill lichSkill;
}
