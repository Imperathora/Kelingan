using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cinemachine;



public class InvertCamera : MonoBehaviour
{
    [SerializeField] private Axis m_axis = default;

    [SerializeField] Toggle m_Toggle;
    public static bool m_invertedX;
    public static bool m_invertedY;
    public static event System.Action<bool> OnToggledX;
    public static event System.Action<bool> OnToggledY;

    public enum Axis
    {
        XAxis,
        YAxis
    }

    public void ToggleValueChanged()
    {
        switch (m_axis)
        {
            case Axis.XAxis:
                if (m_Toggle.isOn)
                {
                    m_invertedX = true;
                    SetToggleBool("_isToggledX", m_invertedX ? 1 : 0);

                    OnToggledX.Invoke(true);
                }

                if (!m_Toggle.isOn)
                {
                    m_invertedX = false;
                    SetToggleBool("_isToggledX", m_invertedX ? 1 : 0);

                    OnToggledX.Invoke(false);
                }

                break;

            case Axis.YAxis:
                if (m_Toggle.isOn)
                {
                    m_invertedY = true;
                    SetToggleBool("_isToggledY", m_invertedY ? 1 : 0);

                    OnToggledY.Invoke(true);
                }

                if (!m_Toggle.isOn)
                {
                    m_invertedY = false;
                    SetToggleBool("_isToggledY", m_invertedY ? 1 : 0);

                    OnToggledY.Invoke(false);
                }
                break;
        }
    }

    public void PlayerPreferences()
    {
        switch (m_axis)
        {
            case Axis.YAxis:
                m_invertedY = (PlayerPrefs.GetInt("_isToggledY") != 0);
                m_Toggle.isOn = m_invertedY;
                OnToggledY.Invoke(m_Toggle.isOn);
                break;

            case Axis.XAxis:
                m_invertedX = (PlayerPrefs.GetInt("_isToggledX") != 0);
                m_Toggle.isOn = m_invertedX;
                OnToggledX.Invoke(m_Toggle.isOn);
                break;
        }
    }

    private void SetToggleBool(string _sliderName, int _value)
    {
        PlayerPrefs.SetInt("_isToggledY", (m_invertedY ? 1 : 0));
        PlayerPrefs.SetInt("_isToggledX", (m_invertedX ? 1 : 0));
    }

}
