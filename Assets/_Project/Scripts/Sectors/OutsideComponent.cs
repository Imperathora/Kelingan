using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Sectors
{
    public class OutsideComponent : MonoBehaviour
    {
        [SerializeField] private GameObject m_container = default;

        private void Start() => WorldComponent.Instance.OnInitialization += Initialize;

        private void Initialize()
        {
            SectorTracker tracker = WorldComponent.Instance?.GetPlayerCharacter()?.GetSectorTracker();
            if (tracker == null)
            {
                Debug.LogError("No SectorTracker found!", this);
            }

            tracker.OnWorldEnter += Activate;
            tracker.OnWorldExit += Deactivate;
        }

        private void Activate() => m_container.SetActive(true);
        private void Deactivate() => m_container.SetActive(false);

        private void OnDestroy()
        {
            SectorTracker tracker = WorldComponent.Instance?.GetPlayerCharacter()?.GetSectorTracker();
            if (tracker)
            {
                tracker.OnWorldEnter -= Activate;
                tracker.OnWorldExit -= Deactivate;
            }
        }
    }
}