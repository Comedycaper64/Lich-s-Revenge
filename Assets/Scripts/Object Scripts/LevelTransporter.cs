using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransporter : MonoBehaviour
{
    //Allows moving between levels. Loads next level if player walks into it
    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerStateMachine>())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);      
        }
    }
}
