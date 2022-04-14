using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfiguration : MonoBehaviour
{

    [SerializeField] private bool m_lookAt = true;
    [SerializeField] private bool m_twodMovement = false;

    public bool GetLookAt() => m_lookAt;
    public bool GetTwodMovement() => m_twodMovement;
}
