using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Image uiImage;

    private Color activeColour = new Color(255f, 255f, 255f, 1);
    private Color inactiveColour = new Color(80f, 80f, 80f, 0.1f);

    public float GetSliderValue()
    {
        return cooldownSlider.value;
    }

    public void SetImageActive(bool active)
    {
        if (active)
        {
            uiImage.color = activeColour;
        }
        else
        {
            uiImage.color = inactiveColour;
        }
    }

    public void SetCooldownSliderValue(float newValue)
    {
        cooldownSlider.value = newValue;
    }
    public void SetManaSliderValue(float newValue)
    {
        manaSlider.value = newValue;
    }
}
