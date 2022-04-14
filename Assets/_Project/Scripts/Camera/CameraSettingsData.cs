using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Settings", menuName = "Settings/Camera Settings Data")]
public class CameraSettingsData : ScriptableObject
{
    [SerializeField] private float m_horizontalMouseSensitivity = 1f;
    [SerializeField] private float m_verticalMouseSensitivity = 1f;
    [SerializeField] private float m_horizontalJoystickSensitivity = 1f;
    [SerializeField] private float m_verticalJoystickSensitivity = 1f;
    [SerializeField] private bool m_isInverted = false;

    public float HorizontalMouseSensitivity { get { return m_horizontalMouseSensitivity; } }
    public float VerticalMouseSensitivity { get { return m_verticalMouseSensitivity; } }
    public float HorizontalJoystickSensitivity { get { return m_horizontalJoystickSensitivity; } }
    public float VerticalJoystickSensitivity { get { return m_verticalJoystickSensitivity; } }
    public bool IsInverted { get { return m_isInverted; } }

}