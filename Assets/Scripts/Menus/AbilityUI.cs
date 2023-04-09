using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class used by the abilities in the UI. Controls transparency of the UI element, along with what button prompt is displayed.
//Two sliders are used to mask a colour over the ability UI if the ability is on cooldown or the player doesn't have sufficient mana
public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Image uiImage;
    [SerializeField] private Image controlImage;


    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite xboxSprite;
    [SerializeField] private Sprite playstationSprite;

    //private Sprite currentUI;

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
            controlImage.color = activeColour;
        }
        else
        {
            uiImage.color = inactiveColour;
            controlImage.color = inactiveColour;
        }
    }

    public void SetKeyboardUI()
    {
        controlImage.sprite = keyboardSprite;
    }

    public void SetXboxUI()
    {
        controlImage.sprite = xboxSprite;
    }

    public void SetPlaystationUI()
    {
        controlImage.sprite = playstationSprite;
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
