using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransporter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }        
    }
}
