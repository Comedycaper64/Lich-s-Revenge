using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gameIsPaused;
    //Set this as main menu if using as main menu manager
    [SerializeField] private GameObject currentOpenScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject deathScreen;

    private void Start() 
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;  
        }  
    }

    public void OpenMenu()
    {
        if (!menuScreen.activeInHierarchy)
        {
            menuScreen.SetActive(true);
            currentOpenScreen = menuScreen;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
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
        currentOpenScreen.SetActive(false);
        currentOpenScreen = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        Time.timeScale = 1f;
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
