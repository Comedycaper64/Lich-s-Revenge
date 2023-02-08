using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueAddSkill", fileName = "NewDialogueAddItem")]
public class DialogueAddSkill : ConversationNode
{
    public enum LichSkill
    {
        firebolt,
        fireball,
        aim,
        dash,
        heal,
        absorb,
        mine,
    }

    public LichSkill lichSkill;
}
