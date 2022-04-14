using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Interaction;
using Framework.Events;

namespace OOB.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class SwitchInteractorComponent : InteractorComponent
    {
        [SerializeField] private GameEvent OnInteractionInput = default;
    
        private void OnEnable()
        {
            OnInteractionInput.Register(Interact);
        }

        private void OnDisable()
        {
            OnInteractionInput.Deregister(Interact);
        }

        private void OnTriggerEnter(Collider other)
        {

            EnterInteractionArea(other.GetComponent<IInteractable>());
        }

        private void OnTriggerExit(Collider other)
        {
            ExitInteractionArea(other.GetComponent<IInteractable>());
        }
    }
}