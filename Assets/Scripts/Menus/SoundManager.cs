using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Stores volume settings and controls music in a scene. Instanced so that all scripts can make use of it
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sceneMusic;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        //A singleton object persists between scenes
        SetUpSingleton();
        if (Instance != null)
        {
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

    //Changes music when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > sceneMusic.Length - 1) {return;}

        audioSource.clip = sceneMusic[scene.buildIndex];
        audioSource.Play();
        if ((scene.buildIndex == 5))
        {
            audioSource.Stop();
        }
    }

    public void PlayAudioSource()
    {
        audioSource.Play();
    }

    public void SetMusicTrack(AudioClip audioTrack)
    {
        audioSource.clip = audioTrack;
        PlayAudioSource();
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;    
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        return masterVolume * musicVolume * 0.3f;
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

    private void SoundSlider_OnAnySliderChanged(object sender, SliderStruct changedSlider)
    {
        switch (changedSlider.GetSoundSlider())
        {
            case SliderStruct.SoundType.Master:
                SetMasterVolume(changedSlider.GetValue());
                break;
            case SliderStruct.SoundType.Music:
                SetMusicVolume(changedSlider.GetValue());
                break;
            case SliderStruct.SoundType.SFX:
                SetSoundEffectVolume(changedSlider.GetValue());
                break;    
        }
    }

}
