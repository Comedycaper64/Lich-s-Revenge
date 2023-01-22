using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderStruct
{
    public enum SliderType
    {
        Master,
        Music,
        SFX,
        LookX,
        LookY,
        AimX,
        AimY,
    } 
   
    private SliderType slider;
    private float value;

    public SliderStruct(SliderType slider, float value)
    {
        this.slider = slider;
        this.value = value;
    }

    public SliderType GetSlider()
    {
        return slider;
    }

    public float GetValue()
    {
        return value;
    }

}
