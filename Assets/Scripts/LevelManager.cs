using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //[SerializeField] private GameObject levelButton;
    //private List<GameObject> enemies;
    private void Awake() 
    {   
        //enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (SceneManager.GetActiveScene().buildIndex != 3)
            {
                LoadNextLevel();
            }
            else
            {
                LoadMainMenu();
            }
        } 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        } 
    }
}
