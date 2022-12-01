using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichAegis : MonoBehaviour
{
    public bool canEnable = true;

    public void ToggleCanEnable(bool canEnable)
    {
        this.canEnable = canEnable;
    }

    public void ToggleAegis(bool enable)
    {
        if (canEnable)
        {
            gameObject.SetActive(enable);
        }
    }
}
