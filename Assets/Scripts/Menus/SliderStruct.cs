using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderStruct
{
    public enum SoundType
    {
        Master,
        Music,
        SFX,
    }

    public enum OptionType
    {
        LookX,
        LookY,
        AimX,
        AimY,
    } 

    public enum DifficultyType
    {
        PlayerAttack,
        PlayerHealth,
        PlayerSpeed,
        EnemyAttack,
        EnemyHealth,
        EnemySpeed,
    }
   
    private SoundType soundSlider;
    private OptionType optionSlider;
    private DifficultyType difficultySlider;
    private float value;

    public SliderStruct(SoundType slider, float value)
    {
        this.soundSlider = slider;
        this.value = value;
    }
    public SliderStruct(OptionType slider, float value)
    {
        this.optionSlider = slider;
        this.value = value;
    }
    public SliderStruct(DifficultyType slider, float value)
    {
        this.difficultySlider = slider;
        this.value = value;
    }

    public SoundType GetSoundSlider()
    {
        return soundSlider;
    }

    public OptionType GetOptionSlider()
    {
        return optionSlider;
    }

    public DifficultyType GetDifficultySlider()
    {
        return difficultySlider;
    }

    public float GetValue()
    {
        return value;
    }

}
