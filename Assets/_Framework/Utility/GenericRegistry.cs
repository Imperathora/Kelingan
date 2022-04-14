using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public abstract class GenericRegistry<T> where T : GenericRegEntry
    {
        protected Dictionary<string, T> m_registeredEntries;

        public event System.Action OnInitialization;
        public event System.Action<T> OnRegistration;

        public virtual void Initialize()
        {
            m_registeredEntries = new Dictionary<string, T>();
            OnInitialization?.Invoke();
            OnInitialization = null;
        }

        public void TryRegister(T _entry)
        {
            if (m_registeredEntries != null)
            {
                Register(_entry);
            }
            else
            {
                OnInitialization += () => Register(_entry);
            }
        }

        private void Register(T _entry)
        {
            if (m_registeredEntries.ContainsKey(_entry.GetID()))
            {
                T entry;
                m_registeredEntries.TryGetValue(_entry.GetID(), out entry);

                if (entry is null)
                {

                    Debug.LogWarning($"Reference was lost (ID: {_entry.GetID()}). Renewing reference.");
                    m_registeredEntries.Remove(_entry.GetID());
                    m_registeredEntries.Add(_entry.GetID(), _entry);
                }
                else if (entry == _entry)
                {
                    Debug.LogWarning($"Entry is already registered (ID: {_entry.GetID()}).");
                }
                else
                {
                    Debug.LogError($"Duplicate ID: {_entry.GetID()} ({_entry.name}, {entry.name})");
                    return;
                }
            }
            else
            {
                m_registeredEntries.Add(_entry.GetID(), _entry);
            }

            OnRegistration?.Invoke(_entry);
        }

        public void Deregister(T _entry)
        {
            if (m_registeredEntries.ContainsKey(_entry.GetID()))
            {
                T entry;
                m_registeredEntries.TryGetValue(_entry.GetID(), out entry);

                if (entry == null || entry != _entry)
                {
                    Debug.LogWarning($"Reference lost or mismatched (ID: {_entry.GetID()}). Removing ID.");
                    m_registeredEntries.Remove(_entry.GetID());
                }
                else if (entry == _entry)
                {
                    m_registeredEntries.Remove(_entry.GetID());
                    Debug.Log($"Deregistered entry (ID: {_entry.GetID()}).");
                }
            }
        }

        public T GetEntry(string _ID)
        {
            if (!m_registeredEntries.TryGetValue(_ID, out T entry))
            {
                Debug.LogError("No entry found (ID: " + _ID + ")");
            }

            return entry;
        }
    }
}