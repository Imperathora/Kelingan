using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Data;
using OOB;

public class CameraAnchor : MonoBehaviour
{
    [SerializeField] private CameraSettingsData m_cameraSettings = default;
    [SerializeField] private CameraBalancingData m_cameraBalancing = default;
    [SerializeField] private FloatContainer m_cameraHorizontalInput = default;
    [SerializeField] private FloatContainer m_cameraVerticalInput = default;
    [SerializeField] private FloatContainer m_mouseHorizontalInput = default;
    [SerializeField] private FloatContainer m_mouseVerticalInput = default;
    [SerializeField] private Vector3Container m_cameraDirection = default;
    private bool IsPlayerInWorld => m_tracker.IsInWorld;

    [SerializeField] private bool m_canLockCursor = true;

    private Transform m_playerTransform = default;

    private OOB.Sectors.SectorTracker m_tracker = default;

    private void Start()
    {
        //m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //if (m_playerTransform == null)
        //{
        //    Debug.Log("CameraAnchor couldn't find a player");
        //}

        // Should be moved elsewhere

        TryGetPlayer();
        if (m_canLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void TryGetPlayer()
    {
        OOB.Player.PlayerCharacter player = WorldComponent.Instance?.GetPlayerCharacter();
        if (player)
        {
            m_playerTransform = player.transform;
            m_tracker = player.GetSectorTracker();
        }
        else
        {
            WorldComponent.Instance.OnInitialization += TryGetPlayer;
        }
    }

    private void Update()
    {
        if (m_playerTransform)
        {
            MoveAndRotateCamera();

            m_cameraDirection.SetValue(transform.forward);
        }
    }

    private void MoveAndRotateCamera()
    {
        float horizontal;
        float vertical;
        if (m_mouseHorizontalInput.Value == 0f && m_mouseVerticalInput.Value == 0f)
        {
            horizontal = m_cameraHorizontalInput.Value * m_cameraSettings.HorizontalJoystickSensitivity * m_cameraBalancing.HorizontalJoystickSpeed;
            vertical = m_cameraVerticalInput.Value * m_cameraSettings.VerticalJoystickSensitivity * m_cameraBalancing.VerticalJoystickSpeed;
        }
        else
        {
            horizontal = m_mouseHorizontalInput.Value * m_cameraSettings.HorizontalMouseSensitivity * m_cameraBalancing.HorizontalMouseSpeed;
            vertical = m_mouseVerticalInput.Value * m_cameraSettings.VerticalMouseSensitivity * m_cameraBalancing.VerticalMouseSpeed;
        }


        vertical = ClampVerticalAngle(m_cameraSettings.IsInverted ? vertical : -vertical);

        transform.SetPositionAndRotation(m_playerTransform.position, Quaternion.Euler(vertical,
            transform.rotation.eulerAngles.y + horizontal, 0f));
    }

    private float ClampVerticalAngle(float _input)
    {
        float verticalAngle = transform.rotation.eulerAngles.x + _input;
        // Check readability
        float maxCameraAngle = IsPlayerInWorld ? m_cameraBalancing.MaxCameraAngle : m_cameraBalancing.MaxCameraAngleVoid;
        float minCameraAngle = IsPlayerInWorld ? m_cameraBalancing.MinCameraAngle : m_cameraBalancing.MinCameraAngleVoid;

        if (verticalAngle > maxCameraAngle && verticalAngle < 180f)
        {
            verticalAngle = maxCameraAngle;
        }
        else if (verticalAngle < 360f - minCameraAngle && verticalAngle > 180f)
        {
            verticalAngle = 360f - minCameraAngle;
        }

        return verticalAngle;
    }
}
