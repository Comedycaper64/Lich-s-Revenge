using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance {get; private set;}

    private bool isStoryMode;

    private CinemachinePOV lookPOV;
    private CinemachinePOV aimPOV;

    private float lookXSensitivity = 150f;
    private float lookYSensitivity = 100f;
    private float aimXSensitivity = 80f;
    private float aimYSensitivity = 50f;

    private float playerAttack = 1f;
    private float playerHealth = 1f;
    private float playerSpeed = 1f;
    private float enemyAttack = 1f;
    private float enemyHealth = 1f;
    private float enemySpeed = 1f;
    private float enemyAttackSpeed = 1f;
    private float enemyStunTime = 1f;

    public event Action OnPlayerHealthChanged;
    public event Action OnEnemyHealthChanged;

    private void Awake() 
    {
        SetUpSingleton();
        if (Instance != null)
        {
            //Debug.LogError("There's more than one OptionsManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SensitivitySlider.OnAnySensitivitySliderChanged += ChangeSensitivity;
        DifficultySlider.OnAnyDifficultySliderChanged += ChangeDifficulty;
    }

    private void ChangeDifficulty(object sender, SliderStruct e)
    {
        switch (e.GetDifficultySlider())
            {
                case SliderStruct.DifficultyType.EnemyAttack:
                    enemyAttack = e.GetValue();
                    break;
                case SliderStruct.DifficultyType.EnemyHealth:
                    enemyHealth = e.GetValue();
                    OnEnemyHealthChanged?.Invoke();
                    break;
                case SliderStruct.DifficultyType.EnemySpeed:
                    enemySpeed = e.GetValue();
                    break;
                case SliderStruct.DifficultyType.EnemyAttackSpeed:
                    enemyAttackSpeed = e.GetValue();
                    break;
                case SliderStruct.DifficultyType.EnemyStunTime:
                    enemyStunTime = e.GetValue();
                    break;
                case SliderStruct.DifficultyType.PlayerAttack:
                    playerAttack = e.GetValue();
                    break;
                case SliderStruct.DifficultyType.PlayerHealth:
                    playerHealth = e.GetValue();
                    OnPlayerHealthChanged?.Invoke();
                    break;
                case SliderStruct.DifficultyType.PlayerSpeed:
                    playerSpeed = e.GetValue();
                    break;
            }
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SensitivitySlider.OnAnySensitivitySliderChanged -= ChangeSensitivity;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject stateCamera = GameObject.FindGameObjectWithTag("StateCamera");

        if (stateCamera)
        {
            CinemachineVirtualCamera[] camera = stateCamera.GetComponentsInChildren<CinemachineVirtualCamera>();

            if (camera.Length >= 2)
            {
                aimPOV = camera[0].GetCinemachineComponent<CinemachinePOV>();
                lookPOV = camera[1].GetCinemachineComponent<CinemachinePOV>();
            }
        }
        
        if (aimPOV && lookPOV)
        {
            SetLookSensitivityX(lookXSensitivity);
            SetLookSensitivityY(lookYSensitivity);
            SetAimSensitivityX(aimXSensitivity);
            SetAimSensitivityY(aimYSensitivity);
        }
    }

    public void SetStoryMode(bool enable)
    {
        isStoryMode = enable;
    }

    public bool IsStoryMode()
    {
        return isStoryMode;
    }

    
    private void ChangeSensitivity(object sender, SliderStruct slider)
    {
        switch(slider.GetOptionSlider())
        {
            case(SliderStruct.OptionType.LookY):
                SetLookSensitivityY(slider.GetValue());
                break;
            case(SliderStruct.OptionType.LookX):
                SetLookSensitivityX(slider.GetValue());
                break;
            case(SliderStruct.OptionType.AimX):
                SetAimSensitivityX(slider.GetValue());
                break;
            case(SliderStruct.OptionType.AimY):
                SetAimSensitivityY(slider.GetValue());
                break;  
        }
    }

    public void SetLookSensitivityX(float speed)
    {
        if (lookPOV)
        {
            lookPOV.m_HorizontalAxis.m_MaxSpeed = speed;
        }
        lookXSensitivity = speed;
    }

    public void SetLookSensitivityY(float speed)
    {
        if (lookPOV)
        {
            lookPOV.m_VerticalAxis.m_MaxSpeed = speed;
        }
        lookYSensitivity = speed;
    }

    public void SetAimSensitivityX(float speed)
    {
        if (aimPOV)
        {
            aimPOV.m_HorizontalAxis.m_MaxSpeed = speed; 
        }
        aimXSensitivity = speed;
    }

    public void SetAimSensitivityY(float speed)
    {
        if (aimPOV)
        {
            aimPOV.m_VerticalAxis.m_MaxSpeed = speed;
        }
        aimYSensitivity = speed;
    }

    public float GetLookXSensitivity()
    {
        return lookXSensitivity;
    }
    public float GetLookYSensitivity()
    {
        return lookYSensitivity;
    }
    public float GetAimXSensitivity()
    {
        return aimXSensitivity;
    }
    public float GetAimYSensitivity()
    {
        return aimYSensitivity;
    }

    public float GetEnemyAttack()
    {
        return enemyAttack;
    }

    public float GetEnemyHealth()
    {
        return enemyHealth;
    }

    public float GetEnemySpeed()
    {
        return enemySpeed;   
    }

    public float GetEnemyAttackSpeed()
    {
        return enemyAttackSpeed;
    }

    public float GetEnemyStunTime()
    {
        return enemyStunTime;
    }

    public float GetPlayerAttack()
    {
        return playerAttack;
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }
}
