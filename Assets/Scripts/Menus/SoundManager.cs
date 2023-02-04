using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        SetUpSingleton();
        if (Instance != null)
        {
            //Debug.LogError("There's more than one SoundManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        masterVolume = 1f;
        musicVolume = 1f;
        sfxVolume = 1f;
        audioSource.volume = GetMusicVolume();
        SoundSlider.OnAnySoundSliderChanged += SoundSlider_OnAnySliderChanged;
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetJustMusicValue()
    {
        return musicVolume;
    }

    public float GetJustSFXValue()
    {
        return sfxVolume;
    }

    public float GetMusicVolume()
    {
        return masterVolume * musicVolume;
    }

    public float GetSoundEffectVolume()
    {
        return masterVolume * sfxVolume;
    }

    public void SetMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
        if (audioSource)
        {
            audioSource.volume = GetMusicVolume();
        }
    }

    public void SetMusicVolume(float musicVolume)
    {
        this.musicVolume = musicVolume;
        if (audioSource)
        {
            audioSource.volume = GetMusicVolume();
        }
    }

    public void SetSoundEffectVolume(float sfxVolume)
    {
        this.sfxVolume = sfxVolume;
    }

    private void SoundSlider_OnAnySliderChanged(object sender, SliderStruct e)
    {
        switch (e.GetSlider())
        {
            case SliderStruct.SliderType.Master:
                SetMasterVolume(e.GetValue());
                break;
            case SliderStruct.SliderType.Music:
                SetMusicVolume(e.GetValue());
                break;
            case SliderStruct.SliderType.SFX:
                SetSoundEffectVolume(e.GetValue());
                break;    
        }
    }

}
