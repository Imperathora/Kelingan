using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public event Action Event;

        public void Invoke()
        {
            if (Event != null)
            {
                Event.Invoke();
            }
        }

        public void Register(Action _listener)
        {
            Event += _listener;
        }

        public void Deregister(Action _listener)
        {
            Event -= _listener;
        }
    }
}