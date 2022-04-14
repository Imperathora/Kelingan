using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OOB.Enemy.Statemachine
{
    public class EnemyStateMachine
    {
        private Dictionary<Type, BaseState> m_states;

        public BaseState CurrentState { get; private set; }

        public void Initialize(Dictionary<Type, BaseState> _states, Type _initialState)
        {
            m_states = _states;

            CurrentState = m_states[_initialState];

            CurrentState.OnStateFinished += SwitchToNewState;
            CurrentState.EnterState();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public void SwitchToNewState(Type _newState)
        {
            CurrentState.OnStateFinished -= SwitchToNewState;
            CurrentState.ExitState();

            CurrentState = m_states[_newState];

            CurrentState.OnStateFinished += SwitchToNewState;
            CurrentState.EnterState();
        }
    }
}