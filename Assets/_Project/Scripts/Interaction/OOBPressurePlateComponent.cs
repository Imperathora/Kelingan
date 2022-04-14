using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Interaction;

namespace OOB.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class OOBPressurePlateComponent : PressurePlateComponent
    {
        private void OnTriggerEnter(Collider other)
        {
            AddActivator(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            RemoveActivator(other.gameObject);
        }
    }
}