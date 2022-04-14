using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Data;
using OOB;
using OOB.Player;
using OOB.Sectors;
using Cinemachine;
using System;
using Rewired.Data.Mapping;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Vector3Container m_cameraDirection = default;
    [SerializeField] private CinemachineFreeLook m_inboundCam;
    [SerializeField] private CinemachineFreeLook m_outboundCam;
    [SerializeField] private InvertCamera m_toggleYAxis;
    [SerializeField] private InvertCamera m_toggleXAxis;
    [SerializeField] private ChangeMouseSensitivity m_sensitivity;

    private PlayerCharacter m_player;
    private PlayerCollector m_playerCollector;
    private Transform m_playerTransform = default;
    private SectorTracker m_tracker = default;
    private CameraSwitch m_cameraSwitch;
    private InvertCamera.Axis m_axis;
    private bool m_isInverted = false;
    private CinemachineFreeLook m_celebrationCam;

    public static CameraManager Instance;

    public CinemachineFreeLook GetVoidCamera => m_outboundCam;
    private void Update()
    {

        if (m_playerTransform == null)
            return;

        m_cameraDirection.SetValue(Camera.main.transform.forward);

        if (m_player.IsInVoid())
        {
            CameraFollowPlayer(m_outboundCam);
            m_inboundCam.Priority = 1;
            m_outboundCam.Priority = 2;

        }
        else
        {
            CameraFollowPlayer(m_inboundCam);
            m_outboundCam.Priority = 1;
            m_inboundCam.Priority = 2;
        }

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        TryGetPlayer();

        InvertCamera.OnToggledX += InvertXAxis;
        InvertCamera.OnToggledY += InvertYAxis;
        ChangeMouseSensitivity.OnValueChanged += SetMouseSpeed;

        m_toggleYAxis.PlayerPreferences();
        m_toggleXAxis.PlayerPreferences();
        m_sensitivity.PlayerPreferences();
    }

    private void SetMouseSpeed(float _speed)
    {
        m_inboundCam.m_YAxis.m_AccelTime = _speed;
        m_inboundCam.m_XAxis.m_AccelTime = _speed / 0.5f;

        m_inboundCam.m_YAxis.m_MaxSpeed = _speed * 3.5f;
        m_inboundCam.m_XAxis.m_MaxSpeed = _speed * 1000;

        m_outboundCam.m_YAxis.m_AccelTime = _speed;
        m_outboundCam.m_XAxis.m_AccelTime = _speed / 0.5f;

        m_outboundCam.m_YAxis.m_MaxSpeed = _speed * 3.5f;
        m_outboundCam.m_XAxis.m_MaxSpeed = _speed * 1000;
    }

    private void OnDestroy()
    {
        m_cameraSwitch.OnCameraSwitchStart -= CameraSwitchStart;
        m_playerCollector.OnStarCollected -= SwitchCelebrationCameraIn;
        m_player.GetCharacterWorldMovement().OnStopCelebrating -= SwitchCelebrationCameraOut;

        InvertCamera.OnToggledX -= InvertXAxis;
        InvertCamera.OnToggledY -= InvertYAxis;
        ChangeMouseSensitivity.OnValueChanged -= SetMouseSpeed;
    }

    private void CameraFollowPlayer(CinemachineFreeLook _cam)
    {
        _cam.LookAt = m_playerTransform;
        _cam.Follow = m_playerTransform;
    }

    private void InvertXAxis(bool _invert)
    {
        if (_invert)
        {
            m_inboundCam.m_XAxis.m_InvertInput = true;
            m_outboundCam.m_XAxis.m_InvertInput = true;
            m_isInverted = true;
        }
        else
        {
            m_inboundCam.m_XAxis.m_InvertInput = false;
            m_outboundCam.m_XAxis.m_InvertInput = false;
            m_isInverted = false;
        }
    }

    private void InvertYAxis(bool _invert)
    {
        if (_invert)
        {
            m_inboundCam.m_YAxis.m_InvertInput = true;
            m_outboundCam.m_YAxis.m_InvertInput = true;
        }
        else
        {
            m_inboundCam.m_YAxis.m_InvertInput = false;
            m_outboundCam.m_YAxis.m_InvertInput = false;
        }
    }

    private void TryGetPlayer()
    {
        m_player = WorldComponent.Instance?.GetPlayerCharacter();

        if (m_player)
        {
            m_playerCollector = m_player.GetComponentInChildren<PlayerCollector>();
            m_celebrationCam = m_player.GetComponentInChildren<CinemachineFreeLook>();
            m_playerTransform = m_player.transform;
            m_tracker = m_player.GetSectorTracker();
            m_cameraSwitch = m_player.GetCameraSwitch();
            m_cameraSwitch.OnCameraSwitchStart += CameraSwitchStart;
            m_playerCollector.OnStarCollected += SwitchCelebrationCameraIn;
            m_player.GetCharacterWorldMovement().OnStopCelebrating += SwitchCelebrationCameraOut;
        }
        else
        {
            WorldComponent.Instance.OnInitialization += TryGetPlayer;
        }
    }

    private void CameraSwitchStart(CinemachineVirtualCamera _cinecam, bool _lookAt)
    {
        if (_lookAt)
            _cinecam.LookAt = m_playerTransform;

        _cinecam.Follow = m_playerTransform;
    }

    private void SwitchCelebrationCameraIn()
    {
        m_celebrationCam.Priority = 3;
        m_inboundCam.Priority = 1;
    }

    private void SwitchCelebrationCameraOut()
    {
        m_celebrationCam.Priority = 1;
        m_inboundCam.Priority = 3;
    }

}
