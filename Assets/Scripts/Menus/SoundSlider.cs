using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private SliderStruct.SoundType sliderType;

    public static event EventHandler<SliderStruct> OnAnySoundSliderChanged;

    private void Start() 
    {
        if (!SoundManager.Instance) {return;}

        switch (sliderType)
        {
            case SliderStruct.SoundType.Master:
                GetComponent<Slider>().value = SoundManager.Instance.GetMasterVolume();
                break;
            case SliderStruct.SoundType.Music:
                GetComponent<Slider>().value = SoundManager.Instance.GetJustMusicValue();
                break;
            case SliderStruct.SoundType.SFX:
                GetComponent<Slider>().value = SoundManager.Instance.GetJustSFXValue();
                break;    
        }

    }

    public void OnSliderChanged()
    {
        OnAnySoundSliderChanged?.Invoke(this, new SliderStruct(sliderType, GetComponent<Slider>().value));
    }
}
