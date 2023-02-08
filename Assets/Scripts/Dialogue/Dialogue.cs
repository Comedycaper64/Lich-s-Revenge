using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Dialogue", fileName = "NewDialogue")]
public class Dialogue : ConversationNode
{
	public string characterName;
	public Sprite[] characterImages;

	public AudioClip characterTalkSound;

	public bool leftSideCharacterImage;
	public bool rightSideCharacterImage;

	[TextArea(3, 10)]
	public string[] sentences;
}
