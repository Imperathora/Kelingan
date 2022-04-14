using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Events;
using Framework.Data;
using Framework.Input;

namespace OOB.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private FloatContainer m_moveHorizontalInput = default;
        [SerializeField] private FloatContainer m_moveVerticalInput = default;
        [SerializeField] public GameEvent OnJumpPressed = default;
        [SerializeField] public GameEvent OnJumpReleased = default;
        [SerializeField] public GameEvent OnDashPressed = default;
        [SerializeField] public GameEvent OnDashReleased = default;
        [SerializeField] public GameEvent OnVoidPressed = default;
        [SerializeField] public GameEvent OnVoidReleased = default;
        [SerializeField] public GameEvent OnDownPressed = default;
        [SerializeField] public GameEvent OnDownReleased = default;
        [SerializeField] public GameEvent OnSprintPressed = default;
        [SerializeField] public GameEvent OnSprintReleased = default;
        [SerializeField] private CharacterWorldMovement m_characterWorldMovement;
        [SerializeField] private InputHandler m_inputHandler;
       
        public bool m_playerInVoid { get; set; }
        public bool m_isInputDisabled { get; set; }

        public void DisableInput(bool _input, bool _inVoid)
        {
            m_playerInVoid = _inVoid;

            if (_input)
            {
                 m_isInputDisabled = true;
                
            }
            else
            {
                m_isInputDisabled = false;
            }

        }
  

        public float HorizontalInput => m_moveHorizontalInput.Value;
        public float VerticalInput => m_moveVerticalInput.Value;

        private Vector2 m_moveInput;
        public Vector2 MoveInput
        {
            get
            {
                if (m_isInputDisabled && m_playerInVoid)
                {
                    m_moveInput = Vector2.zero;
                }
                else if (m_isInputDisabled && !m_playerInVoid)
                {
                    m_characterWorldMovement.enabled = false;
                }

                if(!m_isInputDisabled && m_playerInVoid)
                {
                    m_moveInput.Set(m_moveHorizontalInput.Value, m_moveVerticalInput.Value);
                } else if (!m_isInputDisabled && !m_playerInVoid)
                {
                    m_characterWorldMovement.enabled = true;
                    m_moveInput.Set(m_moveHorizontalInput.Value, m_moveVerticalInput.Value);
                }

                return m_moveInput;
            }
        }

        public event Action OnVoidOpened;
        public event Action OnVoidClosed;

        private void OnEnable()
        {
            OnVoidPressed.Event += OpenVoid;
            OnVoidReleased.Event += CloseVoid;
            m_isInputDisabled = true;
        }

        private void OnDisable()
        {
            OnVoidPressed.Event -= OpenVoid;
            OnVoidReleased.Event -= CloseVoid;
        }

        private void OpenVoid()
        {
            OnVoidOpened?.Invoke();
        }

        private void CloseVoid()
        {
            OnVoidClosed?.Invoke();
        }
    }
}