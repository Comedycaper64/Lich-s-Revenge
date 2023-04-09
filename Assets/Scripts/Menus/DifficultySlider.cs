using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.UI;

//Similar to the sound slider, but for the difficulty settings
public class DifficultySlider : MonoBehaviour
{
    [SerializeField] private SliderStruct.DifficultyType sliderType;
    public static event EventHandler<SliderStruct> OnAnyDifficultySliderChanged;

    private void Start() 
    {
        if (!OptionsManager.Instance) {return;}

        switch (sliderType)
        {
            case SliderStruct.DifficultyType.EnemyAttack:
                GetComponent<Slider>().value = OptionsManager.Instance.GetEnemyAttack();
                break;
            case SliderStruct.DifficultyType.EnemyHealth:
                GetComponent<Slider>().value = OptionsManager.Instance.GetEnemyHealth();
                break;
            case SliderStruct.DifficultyType.EnemySpeed:
                GetComponent<Slider>().value = OptionsManager.Instance.GetEnemySpeed();
                break;
            case SliderStruct.DifficultyType.EnemyAttackSpeed:
                GetComponent<Slider>().value = OptionsManager.Instance.GetEnemyAttackSpeed();
                break;
            case SliderStruct.DifficultyType.EnemyStunTime:
                GetComponent<Slider>().value = OptionsManager.Instance.GetEnemyStunTime();
                break;
            case SliderStruct.DifficultyType.PlayerAttack:
                GetComponent<Slider>().value = OptionsManager.Instance.GetPlayerSpeed();
                break;
            case SliderStruct.DifficultyType.PlayerHealth:
                GetComponent<Slider>().value = OptionsManager.Instance.GetPlayerHealth();
                break;
            case SliderStruct.DifficultyType.PlayerSpeed:
                GetComponent<Slider>().value = OptionsManager.Instance.GetPlayerSpeed();
                break;
        }

    }

    public void OnSliderChanged()
    {
        OnAnyDifficultySliderChanged?.Invoke(this, new SliderStruct(sliderType, GetComponent<Slider>().value));
    }
}
