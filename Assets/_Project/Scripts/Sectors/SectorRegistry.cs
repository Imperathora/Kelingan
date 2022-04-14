using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Sectors
{
    public class SectorRegistry : Framework.GenericRegistry<SectorComponent>
    {
        public event System.Action OnAllSectorsActivated;

        private int m_activeSectorCounter;

        ~SectorRegistry()
        {
            OnAllSectorsActivated = null;
        }

        public SectorComponent GetSector(string _ID) => GetEntry(_ID);

        public Dictionary<string, SectorComponent> GetSectors() => m_registeredEntries;

        public override void Initialize()
        {
            SectorComponent.OnSectorActivatedInVoid += HandleSectorActivatedInVoid;
            base.Initialize();
        }

        private void HandleSectorActivatedInVoid()
        {
            m_activeSectorCounter++;
            if(m_activeSectorCounter >= m_registeredEntries.Count)
            {
                OnAllSectorsActivated?.Invoke();
            }
        }
    }
}