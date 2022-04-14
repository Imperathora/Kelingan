using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OOB.Sectors
{
    public class InnerBoundryComponent : MonoBehaviour
    {
        [SerializeField] private string m_firstID = default;
        [SerializeField] private string m_secondID = default;

        [SerializeField] private UnityEvent OnActivation = default;
        [SerializeField] private UnityEvent OnDeactivation = default;

        private SectorComponent m_firstSector;
        private SectorComponent m_secondSector;

        private int m_lockCounter;

        private void Start()
        {
            WorldComponent.Instance.OnInitialization += Initialize;
        }

        private void Initialize()
        {
            SectorRegistry registry = WorldComponent.Instance.GetSectorRegistry();

            m_firstSector = registry.GetSector(m_firstID);
            m_secondSector = registry.GetSector(m_secondID);

            SectorTracker tracker = WorldComponent.Instance.GetPlayerCharacter()?.GetSectorTracker();
            tracker.OnWorldEnter += DeactivateBoundry;
            tracker.OnWorldExit += ActivateBoundry;

            Player.PlayerInputHandler handler = WorldComponent.Instance?.GetPlayerCharacter()?.GetInputHandler();
            handler.OnVoidOpened += DeactivateBoundry;
            handler.OnVoidClosed += ActivateBoundry;
        }

        private void ActivateBoundry()
        {
            m_lockCounter--;

            if (m_lockCounter < 1
                && m_firstSector.IsActiveInVoid != m_secondSector.IsActiveInVoid)
            {
                OnActivation.Invoke();
            }
        }

        private void DeactivateBoundry()
        {
            m_lockCounter++;

            if (m_lockCounter > 0)
                OnDeactivation.Invoke();
        }
    }
}