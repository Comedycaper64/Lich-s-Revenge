using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<LichBones>(out LichBones lichBones))
        {
            lichBones.AddBone();
            Destroy(gameObject);
        }    
    }
}
