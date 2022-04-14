using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OOB.Sectors
{
    public class OuterBoundryComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnActivation = default;
        [SerializeField] private UnityEvent OnDeactivation = default;

        private void Start()
        {
            WorldComponent.Instance.OnInitialization += () =>
            {
                Player.PlayerInputHandler handler = WorldComponent.Instance?.GetPlayerCharacter()?.GetInputHandler();
                handler.OnVoidOpened += OnDeactivation.Invoke;
                handler.OnVoidClosed += OnActivation.Invoke;
            };
        }
    }
}