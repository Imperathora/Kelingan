using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent m_event = default;
        [SerializeField] private UnityEvent m_response = default;

        private void OnEnable()
        {
            m_event.Event += m_response.Invoke;
        }

        private void OnDisable()
        {
            m_event.Event -= m_response.Invoke;
        }
    }
}