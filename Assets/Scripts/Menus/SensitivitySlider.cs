using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private SliderStruct.SliderType sliderType;
    public static event EventHandler<SliderStruct> OnAnySensitivitySliderChanged;

    private void Start() 
    {
        if (!OptionsManager.Instance) {return;}

        switch (sliderType)
        {
            case SliderStruct.SliderType.LookX:
                GetComponent<Slider>().value = OptionsManager.Instance.GetLookXSensitivity();
                break;
            case SliderStruct.SliderType.LookY:
                GetComponent<Slider>().value = OptionsManager.Instance.GetLookYSensitivity();
                break;
            case SliderStruct.SliderType.AimX:
                GetComponent<Slider>().value = OptionsManager.Instance.GetAimXSensitivity();
                break;    
            case SliderStruct.SliderType.AimY:
                GetComponent<Slider>().value = OptionsManager.Instance.GetAimYSensitivity();
                break;
        }

    }

    public void OnSliderChanged()
    {
        OnAnySensitivitySliderChanged?.Invoke(this, new SliderStruct(sliderType, GetComponent<Slider>().value));
    }
}
