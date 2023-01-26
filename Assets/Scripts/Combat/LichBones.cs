using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LichBones : MonoBehaviour
{
    [SerializeField] private int maxBones;
    [SerializeField] private int bones;
    [SerializeField] public TextMeshProUGUI boneText;

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

    public void AddBone()
    {
        if (bones < maxBones)
        {
            bones++;
            UpdateBoneText();
        }
    }
}
