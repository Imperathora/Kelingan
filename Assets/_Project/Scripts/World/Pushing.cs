using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Player;
using OOB;
using System;

public class Pushing : MonoBehaviour
{


    private PlayerCharacter m_playerCharacter;



    private void OnTriggerEnter(Collider other)
    {
        m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();

        if (m_playerCharacter == null)
            return;

        if (other.transform.IsChildOf(m_playerCharacter.transform))
        {
            m_playerCharacter.PushingObjectStart();

        }
}

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.IsChildOf(m_playerCharacter.transform))
        {
            m_playerCharacter.PushingObjectStop();
        }
    }
}
