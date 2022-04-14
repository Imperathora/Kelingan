using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private Slider m_ambientVolume;
    [SerializeField] private Slider m_musicVolume;
    [SerializeField] private Slider m_soundVolume;
    [SerializeField] private Slider m_masterVolume;

    public void SetMasterVolume(float _volume)
    {
        SetVolume("Master", _volume);
    }

    public void SetMusicVolume(float _volume)
    {
        SetVolume("Music", _volume);
        SetVolume("Music2", _volume);
        SetVolume("Music3", _volume);
        SetVolume("Music4", _volume);
    }


    public void SetAmbientVolume(float _volume)
    {
        SetVolume("Ambient", _volume);
    }

    public void SetSoundsVolume(float _volume)
    {
        SetVolume("Effects", _volume);
        SetVolume("EffectsMenu", _volume);
    }


    public void PlayerPreferences()
    {

        m_masterVolume.value = PlayerPrefs.GetFloat("Master", 0.5f);
        m_ambientVolume.value = PlayerPrefs.GetFloat("Ambient", 0.6f);
        m_musicVolume.value = PlayerPrefs.GetFloat("Music", 0.3f);
        m_soundVolume.value = PlayerPrefs.GetFloat("Effects", 0.3f);
    }

    private void SetVolume(string _sliderName, float _value)
    {
        m_mixer.SetFloat(_sliderName, Mathf.Log10(_value) * 20);
        PlayerPrefs.SetFloat(_sliderName, _value);
    }

}
