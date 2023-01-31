using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LichBones : MonoBehaviour
{
    [SerializeField] private int maxBones;
    [SerializeField] private int bones;
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
        boneText.text = "x " + bones;
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
