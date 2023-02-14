using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySlider : MonoBehaviour
{
    [SerializeField] private SliderStruct.DifficultyType sliderType;
    public static event EventHandler<SliderStruct> OnAnyDifficultySliderChanged;

    private void Start() 
    {
        if (!PlayerStats.Instance || EnemyStats.Instance) {return;}

        switch (sliderType)
        {
            case SliderStruct.DifficultyType.EnemyAttack:
                GetComponent<Slider>().value = EnemyStats.Instance.GetEnemyAttack();
                break;
            case SliderStruct.DifficultyType.EnemyHealth:
                GetComponent<Slider>().value = EnemyStats.Instance.GetEnemyHealth();
                break;
            case SliderStruct.DifficultyType.EnemySpeed:
                GetComponent<Slider>().value = EnemyStats.Instance.GetEnemySpeed();
                break;
            case SliderStruct.DifficultyType.PlayerAttack:
                GetComponent<Slider>().value = PlayerStats.Instance.GetPlayerSpeed();
                break;
            case SliderStruct.DifficultyType.PlayerHealth:
                GetComponent<Slider>().value = PlayerStats.Instance.GetPlayerHealth();
                break;
            case SliderStruct.DifficultyType.PlayerSpeed:
                GetComponent<Slider>().value = PlayerStats.Instance.GetPlayerSpeed();
                break;
        }

    }

    public void OnSliderChanged()
    {
        OnAnyDifficultySliderChanged?.Invoke(this, new SliderStruct(sliderType, GetComponent<Slider>().value));
    }
}
