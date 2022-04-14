using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Balancing Data", menuName = "Balancing/Camera Balancing Data")]
public class CameraBalancingData : ScriptableObject
{
    [SerializeField] private float m_horizontalMouseSpeed = default;
    [SerializeField] private float m_verticalMouseSpeed = default;
    [SerializeField] private float m_horizontalJoystickSpeed = default;
    [SerializeField] private float m_verticalJoystickSpeed = default;
    [SerializeField] private float m_maxViewAngle = default;
    [SerializeField] private float m_minViewAngle = default;
    [SerializeField] private float m_maxViewAngleVoid = default;
    [SerializeField] private float m_minViewAngleVoid = default;

    public float HorizontalMouseSpeed { get { return m_horizontalMouseSpeed; } }
    public float VerticalMouseSpeed { get { return m_verticalMouseSpeed; } }
    public float HorizontalJoystickSpeed { get { return m_horizontalJoystickSpeed; } }
    public float VerticalJoystickSpeed { get { return m_verticalJoystickSpeed; } }
    public float MaxCameraAngle { get { return m_maxViewAngle; } }
    public float MinCameraAngle { get { return m_minViewAngle; } }
    public float MaxCameraAngleVoid { get { return m_maxViewAngleVoid; } }
    public float MinCameraAngleVoid { get { return m_minViewAngleVoid; } }
}
