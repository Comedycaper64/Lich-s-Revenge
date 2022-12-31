using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammererSlam : MonoBehaviour
{
    [SerializeField] private GameObject slamVisual;

    private void Awake() 
    {
        slamVisual.transform.parent = null;    
    }

    public void SetupSlamVisual(float slamRadius)
    {
        slamVisual.transform.localScale = new Vector3(slamRadius * 2, 0.5f, slamRadius * 2);
    }

    public void SetSlamVisualLocation(Vector3 newLocation)
    {
        slamVisual.transform.position = newLocation;
    }

    public void EnableSlamVisual(bool enable)
    {
        slamVisual.SetActive(enable);
    }
}
