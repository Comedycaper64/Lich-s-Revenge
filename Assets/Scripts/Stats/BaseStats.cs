using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    /*
    public static BaseStats Instance {get; private set;}

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one BaseStats! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    */

    public float baseHealth;
    public float baseAttack;
}
