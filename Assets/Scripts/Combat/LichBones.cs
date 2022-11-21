using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBones : MonoBehaviour
{
    [SerializeField] private int maxBones;
    private int bones;

    private void Awake() 
    {
        bones = maxBones;    
    }

    public bool TryUseBones(int bonesToUse)
    {
        if (bonesToUse <= bones)
        {
            bones -= bonesToUse;
            return true;
        }
        return false;
    }

    public void AddBone()
    {
        if (bones < maxBones)
        {
            bones++;
        }
    }
}
