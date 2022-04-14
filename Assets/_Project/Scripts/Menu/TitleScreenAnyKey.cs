using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Events;
using OOB.Player;

public class TitleScreenAnyKey : MonoBehaviour
{
    [SerializeField] private bool m_isActive;
    [SerializeField] private UnityEvent OnAnyKey;
    private Player m_player;

    void Start()
    {
        m_player = ReInput.players.GetPlayer(0);
    }


    void Update()
    {
        if (m_isActive && Input.anyKeyDown)
        {
            OnAnyKey.Invoke();
        }
    }
}