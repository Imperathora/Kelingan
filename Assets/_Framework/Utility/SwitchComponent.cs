using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class SwitchComponent : MonoBehaviour
    {
        protected enum State
        {
            ON,
            OFF
        }

        [HideProperty(VisibilityMode.Editable, VisibilityMode.ReadOnly)] [SerializeField] private State m_defaultState = State.OFF;
        [SerializeField] protected UnityEvent OnActivation;
        [SerializeField] protected UnityEvent OnDeactivation;

        protected State CurrentState { get; private set; }

        public bool IsActivated { get { return CurrentState == State.ON; } }

        protected virtual void Start()
        {
            CurrentState = m_defaultState;
        }

        public virtual void Activate()
        {
            if (IsActivated)
                return;

            CurrentState = State.ON;
            OnActivation.Invoke();
        }

        public virtual void Deactivate()
        {
            if (!IsActivated)
                return;

            CurrentState = State.OFF;
            OnDeactivation.Invoke();
        }

        public void Toggle()
        {
            if (CurrentState == State.ON)
            {
                CurrentState = State.OFF;
                OnDeactivation.Invoke();
            }
            else
            {
                CurrentState = State.ON;
                OnActivation.Invoke();
            }
        }

        public void ResetSwitch()
        {
            CurrentState = m_defaultState;
        }        
    }
}