using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace OOB.Sectors
{
    public class SectorTracker : MonoBehaviour
    {
        public event Action OnWorldEnter;
        public event Action OnWorldExit;

        public bool IsInWorld { get; private set; }

        private GenericLock m_voidLock = new GenericLock(GenericLock.LockState.UNLOCKED);

        private void Start()
        {
            m_voidLock.OnStateChanged += BroadcastChange;
        }


        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(HandleTriggerEnterDelayed(other));
        }

        private void OnTriggerExit(Collider other)
        {

            StartCoroutine(HandleTriggerExitDelayed(other));
        }

        private IEnumerator HandleTriggerExitDelayed(Collider other)
        {
            yield return new WaitForEndOfFrame();
            m_voidLock.Unlock(other);
        }

        private IEnumerator HandleTriggerEnterDelayed(Collider other)
        {
            yield return new WaitForEndOfFrame();
            m_voidLock.Lock(other);
        }

        private void BroadcastChange(GenericLock.LockState _newState)
        {
            if (_newState == GenericLock.LockState.LOCKED)
            {
                IsInWorld = true;
                OnWorldEnter?.Invoke();

            }
            else
            {
                IsInWorld = false;
                OnWorldExit?.Invoke();
            }
        }

        public void ResetTracker()
        {
            m_voidLock.Clear();
        }

        private void OnDestroy()
        {
            OnWorldEnter = null;
            OnWorldExit = null;
        }
    }
}