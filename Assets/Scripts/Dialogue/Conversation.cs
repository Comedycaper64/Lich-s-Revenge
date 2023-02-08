using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Conversation", fileName = "NewConversation")]
public class Conversation : ScriptableObject
{
   public ConversationNode[] conversationNodes;
}
