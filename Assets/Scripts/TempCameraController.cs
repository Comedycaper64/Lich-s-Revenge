using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TempCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineStateDrivenCamera stateCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            stateCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            stateCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        }
        
    }
}
