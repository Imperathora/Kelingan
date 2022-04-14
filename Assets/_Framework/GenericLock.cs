using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class GenericLock
    {
        public enum LockState
        {
            LOCKED,
            UNLOCKED
        }

        public LockState DefaultState { get; private set; }
        public LockState CurrentState { get; private set; }
        public bool IsLocked { get { return CurrentState == LockState.LOCKED; } }
        public bool IsUnlocked { get { return CurrentState == LockState.UNLOCKED; } }

        /// <summary>
        /// Broadcasts new LockState on change of state.
        /// </summary>
        public event System.Action<LockState> OnStateChanged;

        private Dictionary<Object, LockState> m_keys;

        /// <summary>
        /// The default state of a lock is 'locked' but an 'open' lock can be created if desired.
        /// </summary>
        /// <param name="_defaultState"></param>
        public GenericLock(LockState _defaultState = LockState.LOCKED)
        {
            DefaultState = _defaultState;
            CurrentState = DefaultState;
            m_keys = new Dictionary<Object, LockState>();
        }

        /// <summary>
        /// Ads key with state UNLOCKED
        /// </summary>
        /// <param name="_key"></param>
        public void Unlock(Object _key)
        {
            SetLockState(_key, LockState.UNLOCKED);
        }

        /// <summary>
        /// Ads key with state LOCKED
        /// </summary>
        /// <param name="_key"></param>
        public void Lock(Object _key)
        {
            SetLockState(_key, LockState.LOCKED);
        }

        private void SetLockState(Object _key, LockState _state)
        {
            if (m_keys.ContainsKey(_key)) // If lock contains key and it changes state, remove it.
            {
                m_keys.TryGetValue(_key, out LockState state);

                if (_state == state)
                {
                    Debug.LogWarning("Lock is already " + state + " by this key (" + _key.name + ")!");
                }
                else
                {
                    m_keys.Remove(_key);
                    UpdateLockState();
                }
            }
            else // If lock doesn't contain key, add it.
            {
                m_keys.Add(_key, _state);
                UpdateLockState();
            }
        }

        public void Clear()
        {
            m_keys.Clear();
        }

        private void UpdateLockState()
        {
            LockState newState = RecalculateLockState();
            if (CurrentState != newState) // If state has changed broadcast the new state.
            {
                CurrentState = newState;

                if (OnStateChanged != null)
                    OnStateChanged.Invoke(CurrentState);
            }
        }

        private LockState RecalculateLockState()
        {
            Clean();

            int lockedCounter = 0;
            int unlockedCounter = 0;
                       
            foreach (LockState value in m_keys.Values)
            {
                if (value == LockState.LOCKED)
                {
                    lockedCounter++;
                }
                else
                {
                    unlockedCounter++;
                }
            }

            LockState newState;

            if (DefaultState == LockState.LOCKED && unlockedCounter > lockedCounter)
            {
                newState = LockState.UNLOCKED;
            }
            else if (DefaultState == LockState.UNLOCKED && lockedCounter > unlockedCounter)
            {
                newState = LockState.LOCKED;
            }
            else
            {
                newState = DefaultState;
            }

            return newState;
        }

        private void Clean()
        {
            foreach (Object key in m_keys.Keys) // Remove keys that don't exist anymore.
            {
                if (key is null)
                {
                    m_keys.Remove(key);
                }
            }
        }
    }
}