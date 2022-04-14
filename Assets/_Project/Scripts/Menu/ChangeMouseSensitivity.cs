using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMouseSensitivity : MonoBehaviour
{
    [SerializeField] private Slider m_sensitivity;
    public static event System.Action<float> OnValueChanged;


    public void SetSensitivity(float _sensitivity)
    {
        SetMouseSensititivy("MouseY", _sensitivity);
        SetMouseSensititivy("MouseX", _sensitivity);

        OnValueChanged.Invoke(m_sensitivity.value);
    }

    public void PlayerPreferences()
    {
        m_sensitivity.value = PlayerPrefs.GetFloat("MouseY", 0.3f);
        m_sensitivity.value = PlayerPrefs.GetFloat("MouseX", 0.6f);

        OnValueChanged.Invoke(m_sensitivity.value);
    }

    private void SetMouseSensititivy(string _sliderName, float _value)
    {
        PlayerPrefs.SetFloat("MouseX", _value);
        PlayerPrefs.SetFloat("MouseY", _value);
    }

}
