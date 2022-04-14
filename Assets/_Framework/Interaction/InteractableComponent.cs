using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Interaction
{
    public interface IInteractable
    {
        void Interact();
    }

    public class InteractableComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] protected UnityEvent OnInteraction;

        public virtual void Interact()
        {
            OnInteraction.Invoke();
        }
    }
}