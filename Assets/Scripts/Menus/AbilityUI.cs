using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Slider uiSlider;
    [SerializeField] private Image uiImage;

    public float GetSliderValue()
    {
        return uiSlider.value;
    }

    public void SetImageColour(Color newColor)
    {
        uiImage.color = newColor;
    }

    public void SetSliderValue(float newValue)
    {
        uiSlider.value = newValue;
    }
}
