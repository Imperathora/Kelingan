using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OOB.Sectors
{
    public class SectorSwitchComponent : MonoBehaviour
    {
        [SerializeField] protected string m_ID;

        [SerializeField] protected UnityEvent OnActivation;
        //[SerializeField] protected UnityEvent OnDeactivation;
        [SerializeField] protected UnityEvent<bool> OnReset;

        protected SectorComponent m_sector;
        protected bool IsActivated
        {
            get
            {
                return m_sector.IsActiveInVoid;
            }
            private set
            {
                m_sector.SetActiveInVoid(value);
            }
        }

        protected virtual void Start()
        {
            WorldComponent.Instance.OnInitialization += () =>
            { m_sector = WorldComponent.Instance?.GetSectorRegistry()?.GetSector(m_ID); };
        }

        public virtual void Activate()
        {
            if (IsActivated)
                return;

            IsActivated = true;
            OnActivation.Invoke();
                   
            
        }

        //public virtual void Deactivate()
        //{
        //    if (!IsActivated)
        //        return;

        //    IsActivated = false;
        //    OnDeactivation.Invoke();
        //}


        private void ResetSwitch(bool _newState)
        {
            OnReset.Invoke(_newState);
        }
    }
}