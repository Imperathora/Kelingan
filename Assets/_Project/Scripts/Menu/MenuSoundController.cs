using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_startSound;
    [SerializeField] private AudioClip m_pressButtonSound;
    [SerializeField] private AudioClip m_toggleSound;
    [SerializeField] private AudioClip m_switchSound;
    [SerializeField] private AudioClip m_confirmSound;
    [SerializeField] private AudioClip m_titleScreenSound;

    private void Start()
    {
        SelectButton.OnSelected += SwitchButton;
    }
    public void StartPressed()
    {
        PlaySound(m_startSound);
    }

    public void SwitchButton()
    {
        PlaySound(m_switchSound);
    }

    public void AnyKey()
    {
        PlaySound(m_titleScreenSound);
    }

    public void TogglePressed()
    {
        PlaySound(m_toggleSound);
    }

    public void Confirmed()
    {
        PlaySound(m_confirmSound);
    }

    public void ButtonPressed()
    {
        PlaySound(m_pressButtonSound);
    }

    public void Slider()
    {
        if (m_audioSource == null)
            return;

        if (m_audioSource.isPlaying)
            return;
        else
            m_audioSource.PlayOneShot(m_toggleSound);
    }

    private void PlaySound(AudioClip _clip)
    {
        if (m_audioSource == null)
            return;
        m_audioSource.PlayOneShot(_clip);
    }
}
