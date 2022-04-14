using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Interaction
{
    public class PressurePlateComponent : SwitchComponent
    {
        protected List<GameObject> m_activator = new List<GameObject>();

        protected virtual void OnDisable()
        {
            m_activator.Clear();
        }

        protected void AddActivator(GameObject _activator)
        {
            m_activator.Add(_activator);

            Activate();            
        }

        protected void RemoveActivator(GameObject _activator)
        {
            m_activator.Remove(_activator);

            if (m_activator.Count == 0)
            {
                Deactivate();
            }
        }
    }
}