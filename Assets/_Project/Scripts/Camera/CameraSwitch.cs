using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public event Action<CinemachineVirtualCamera, bool> OnCameraSwitchStart;
    public event Action<CinemachineFreeLook> OnCameraSwitchEnd;
    public event Action TwodMovementStarting;
    public event Action TowdMovementStoped;

    CinemachineVirtualCamera vcam;
    private void OnTriggerEnter(Collider other)
    {
        bool lookAt;
        bool towdMovement;

        vcam = other.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        lookAt = other.GetComponent<CameraConfiguration>().GetLookAt();
        towdMovement = other.GetComponent<CameraConfiguration>().GetTwodMovement();

        if (towdMovement)
            TwodMovementStarting?.Invoke();

        vcam.Priority = 3;
        OnCameraSwitchStart?.Invoke(vcam, lookAt);
    }

    private void OnTriggerExit(Collider other)
    {
        vcam.Priority = 1;
        OnCameraSwitchEnd?.Invoke(other.GetComponentInChildren<CinemachineFreeLook>());
        TowdMovementStoped?.Invoke();
    }

    private void OnDestroy()
    {
        TwodMovementStarting = null;
        TowdMovementStoped = null;
    }
}
