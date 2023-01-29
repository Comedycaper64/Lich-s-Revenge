using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance {get; private set;}

    private CinemachinePOV lookPOV;
    private CinemachinePOV aimPOV;

    private float lookXSensitivity = 150f;
    private float lookYSensitivity = 100f;
    private float aimXSensitivity = 80f;
    private float aimYSensitivity = 50f;

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

    
    private void ChangeSensitivity(object sender, SliderStruct slider)
    {
        switch(slider.GetSlider())
        {
            case(SliderStruct.SliderType.LookY):
                SetLookSensitivityY(slider.GetValue());
                break;
            case(SliderStruct.SliderType.LookX):
                SetLookSensitivityX(slider.GetValue());
                break;
            case(SliderStruct.SliderType.AimX):
                SetAimSensitivityX(slider.GetValue());
                break;
            case(SliderStruct.SliderType.AimY):
                SetAimSensitivityY(slider.GetValue());
                break;  
        }
    }

    public void SetLookSensitivityX(float speed)
    {
        lookPOV.m_HorizontalAxis.m_MaxSpeed = speed;
        lookXSensitivity = speed;
    }

    public void SetLookSensitivityY(float speed)
    {
        lookPOV.m_VerticalAxis.m_MaxSpeed = speed;
        lookYSensitivity = speed;
    }

    public void SetAimSensitivityX(float speed)
    {
        aimPOV.m_HorizontalAxis.m_MaxSpeed = speed; 
        aimXSensitivity = speed;
    }

    public void SetAimSensitivityY(float speed)
    {
        aimPOV.m_VerticalAxis.m_MaxSpeed = speed;
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
}
