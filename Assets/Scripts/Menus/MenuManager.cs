using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gameIsPaused;
    //Set this as main menu if using as main menu manager
    [SerializeField] private GameObject currentOpenScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TutorialManager tutorialManager;

    private int currentSceneIndex;

    private void Start() 
    {
        UnpauseGame();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;  
        } 
    }

    private void Update() 
    {
        //if (currentSceneIndex == 0) {return;}

        if (tutorialManager.currentOpenScreen && currentOpenScreen)
        {
            currentOpenScreen.SetActive(false);
        }    
        else if (currentOpenScreen)
        {
            currentOpenScreen.SetActive(true);
        }

        if (gameIsPaused)
        {
            if (InputReader.controllerBeingUsed)
            {
                //Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }   
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; 
            }
        }
    }

    public void OpenMenu()
    {
        if (!menuScreen.activeInHierarchy && !currentOpenScreen)
        {
            menuScreen.SetActive(true);
            currentOpenScreen = menuScreen;
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
        else
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        if (tutorialManager.currentOpenScreen != null) {return;}

        currentOpenScreen.SetActive(false);
        currentOpenScreen = null;
        UnpauseGame();
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
    }

    public void ToggleDeathUI(bool enable)
    {
        deathScreen.SetActive(enable);
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenScreen(GameObject screen)
    {
        if (currentOpenScreen)
        {
            currentOpenScreen.SetActive(false);
        }
        screen.SetActive(true);
        currentOpenScreen = screen;
    }

    public void CloseScreen(GameObject screen)
    {
        screen.SetActive(false);
        currentOpenScreen = null;
    }
    

    public void ExitGame()
    {
        Application.Quit();
    }

}
