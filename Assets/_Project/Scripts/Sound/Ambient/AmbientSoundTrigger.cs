using OOB;
using OOB.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundTrigger : MonoBehaviour
{

    public event Action OnWindFadeOut;
    public event Action OnWindFadeIn;
    PlayerCharacter m_playerCharacter;
    public int m_WindCounter { get; set; }
    private void Start()
    {
        m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();
        m_WindCounter = 0;
    }

    private void OnDestroy()
    {
        OnWindFadeOut = null;
        OnWindFadeIn = null;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (m_playerCharacter.IsInVoid())
        {
            m_WindCounter++;
            return;
        }

        if (other.gameObject.tag == "EnterWindTrigger")
        {
            OnWindFadeIn?.Invoke();
        }

        if (other.gameObject.tag == "ExitWindTrigger")
        {
            OnWindFadeOut?.Invoke();
        }
    }

}
