using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Resource used by the player to do powerful abilities
public class LichBones : MonoBehaviour
{
    [SerializeField] private int maxBones;
    [SerializeField] private int bones;
    //Bone text is the UI element that displays the player's bones
    [SerializeField] public TextMeshProUGUI boneText;
    [SerializeField] public AudioClip boneGetSFX;

    public event Action OnBoneGet;

    private void Start() 
    {
        UpdateBoneText();    
    }

    public bool TryUseBones(int bonesToUse)
    {
        if (bonesToUse <= bones)
        {
            bones -= bonesToUse;
            UpdateBoneText();
            return true;
        }
        return false;
    }

    public int GetBones()
    {
        return bones;
    }

    private void UpdateBoneText()
    {
        boneText.text = bones + " / " + maxBones;
    }

    public void ResetBones()
    {
        bones = 1;
        UpdateBoneText();
    }

    public bool TryAddBone()
    {
        if (bones < maxBones)
        {
            bones++;
            UpdateBoneText();
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(boneGetSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            OnBoneGet?.Invoke();
            return true;
        }
        return false;
    }
}
