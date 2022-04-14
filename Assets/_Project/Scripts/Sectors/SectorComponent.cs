using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;

namespace OOB.Sectors
{
    public class SectorComponent : Framework.GenericRegEntry
    {
        [HideProperty(VisibilityMode.Editable, VisibilityMode.ReadOnly)] [SerializeField] private bool m_defaultState = default;

        [SerializeField] private GameObject m_SectorInternals = default;

        public bool IsActiveInVoid { get; private set; }

        public event Action OnSectorActivation;
        public event Action OnSectorDeactivation;
        public static event Action OnSectorActivatedInVoid;


        private void Start() => Initialize();

        protected override void Initialize()
        {
            IsActiveInVoid = m_defaultState;

            WorldComponent.Instance?.GetSectorRegistry()?.TryRegister(this);
            WorldComponent.Instance.OnInitialization += () =>
            {
                SectorTracker tracker = WorldComponent.Instance?.GetPlayerCharacter()?.GetSectorTracker();
                if (tracker == null)
                {
                    Debug.LogError("No SectorTracker found!", this);
                }

                tracker.OnWorldEnter += ActivateSector;
                tracker.OnWorldExit += DeactivateSector;

                if (m_defaultState)
                {
                    OnSectorActivatedInVoid?.Invoke();
                }
            };
        }

        public void SetActiveInVoid(bool _isActive)
        {
            if (IsActiveInVoid == _isActive)
                return;
            IsActiveInVoid = _isActive;
            OnSectorActivatedInVoid?.Invoke();
        }

        private void ActivateSector()
        {
            m_SectorInternals.SetActive(true);

            OnSectorActivation?.Invoke();
        }

        private void DeactivateSector()
        {
            if (!IsActiveInVoid)
            {
                m_SectorInternals.SetActive(false);

                OnSectorDeactivation?.Invoke();
            }
        }

        public void ResetState()
        {
            IsActiveInVoid = m_defaultState;
        }

        private void OnDestroy()
        {
            OnSectorActivatedInVoid = null;
        }
    }
}