using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Interaction
{
    public class InteractorComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnInteraction = default;

        private IInteractable m_interactable;
        private InteractableComponent m_interactableComponent;
        private bool m_isSwitchActive;
        protected void EnterInteractionArea(IInteractable _interactable)
        {
            if (m_interactable == null
                && _interactable != null)
            {
                m_interactable = _interactable;
                //Debug.Log("Interactor entered InteractionField.");
            }
        }

        protected void ExitInteractionArea(IInteractable _interactable)
        {
            if (m_interactable != null && m_interactable == _interactable)
            {
                m_interactable = null;
                //Debug.Log("Interactor left InteractionField.");
            }
        }


        public virtual void Interact()
        {
            if (m_interactable != null)
            {
                m_interactable.Interact();
                OnInteraction.Invoke();
                m_interactable = null;
            }
        }
    }
}