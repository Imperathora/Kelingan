using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Data;
using System;
using Cinemachine;
using OOB.Sectors;

namespace OOB.Player
{
    public class VoidMovement : PlayerMovementBase
    {
        [SerializeField] private Framework.Data.Vector3Container m_cameraDirection = default;
        [SerializeField] private VoidMovementBalancingData m_movementData = default;
        [SerializeField] private SectorTracker m_sectorTracker = default;

        private PlayerInputHandler m_inputHandler;
        public event Action OnVoidSprintStarted;
        public event Action OnVoidSprintStoped;

        private bool m_isMovingUp;
        private bool m_voidIntro;
        private float m_timer;
        private bool m_isInputDisabled;
        private bool m_canSprint;

        


        private CinemachineFreeLook m_voidCamera;
        private float m_xAxisSpeed;
        private float m_yAxisSpeed;
        private void StartMovingUp() => m_isMovingUp = true;
        private void StopMovingUp() => m_isMovingUp = false;

        private bool m_isMovingDown;
        private void StartMovingDown() => m_isMovingDown = true;
        private void StopMovingDown() => m_isMovingDown = false;

        private bool m_isSprinting;
        private void StartSprint() => m_isSprinting = true;
        private void StopSprint() => m_isSprinting = false;
        public bool GetIsInputDisabled() => m_isInputDisabled;

        public Vector2 MoveInput { get; set; }

        public void Initialize(Rigidbody _rigidbody, PlayerInputHandler _inputHandler)
        {
            m_rigidbody = _rigidbody;
            m_inputHandler = _inputHandler;
            m_voidCamera = CameraManager.Instance?.GetVoidCamera;

        }

        private void OnEnable()
        {
            m_inputHandler.OnJumpPressed.Event += StartMovingUp;
            m_inputHandler.OnJumpReleased.Event += StopMovingUp;
            m_inputHandler.OnDownPressed.Event += StartMovingDown;
            m_inputHandler.OnDownReleased.Event += StopMovingDown;
            m_inputHandler.OnSprintPressed.Event += StartSprint;
            m_inputHandler.OnSprintReleased.Event += StopSprint;
            m_sectorTracker.OnWorldExit += OnVoidEnter;

        }
        private void OnDisable()
        {
            m_inputHandler.OnJumpPressed.Event -= StartMovingUp;
            m_inputHandler.OnJumpReleased.Event -= StopMovingUp;
            m_inputHandler.OnDownPressed.Event -= StartMovingDown;
            m_inputHandler.OnDownReleased.Event -= StopMovingDown;
            m_inputHandler.OnSprintPressed.Event -= StartSprint;
            m_inputHandler.OnSprintReleased.Event -= StopSprint;
           

            m_isMovingUp = false;
            m_isMovingDown = false;
            m_isSprinting = false;

        }

        private void OnDestroy()
        {
            m_sectorTracker.OnWorldExit -= OnVoidEnter;
        }


        private void FixedUpdate() => TurnAndMove();
        public void TurnAndMove()
        {

            Vector3 moveDirection = transform.forward;
            
            if (MoveInput.x != 0 || MoveInput.y != 0)
            {

                if (MoveInput.magnitude > 1)
                    MoveInput = MoveInput.normalized;


                Vector3 newLookDirection = CalcLookDirection(MoveInput, m_cameraDirection.Value);

                Vector3 newTurnDirection = newLookDirection.With(y: 0f);
                TurnTowards(transform.forward, newTurnDirection, m_movementData.TurnMode, m_movementData.TurnSpeed);

                moveDirection = newLookDirection;
            }

            if (m_voidIntro)
            {
                m_timer += Time.fixedDeltaTime;

                if (m_timer < m_movementData.FlyInTimer)
                {
                    m_rigidbody.velocity = CalculateMoveVelocity(transform.forward, new Vector2(m_movementData.FlyInDistance, m_movementData.FlyInDistance));
                    OnVoidSprintStarted?.Invoke();
                    return;
                } 
                    EnableInput();
                    m_voidIntro = false;
                    OnVoidSprintStoped?.Invoke();
                    
            } else
            {

                m_rigidbody.velocity = CalculateMoveVelocity(moveDirection, MoveInput);
            }

        }
        public void CanSprint(bool _cansprint)
        {
            m_canSprint = _cansprint;
        }

        private Vector3 CalculateMoveVelocity(Vector3 _moveDirection, Vector2 _inputAxis)
        {
            Vector3 velocity = _moveDirection * Time.fixedDeltaTime;


            if (m_isSprinting && m_canSprint)
            {
                OnVoidSprintStarted?.Invoke();
                velocity *= m_movementData.SprintAcceleration * _inputAxis.magnitude;
                velocity = Vector3.ClampMagnitude(velocity, m_movementData.SprintAcceleration);

            } else
            {
                OnVoidSprintStoped?.Invoke();
                velocity *= m_movementData.MoveSpeed * _inputAxis.magnitude;
                velocity = Vector3.ClampMagnitude(velocity, m_movementData.MoveSpeed);
            }

            velocity += CalculateVerticalVelocity();
        
            return velocity;
        }

        private Vector3 CalculateVerticalVelocity()
        {
            Vector3 velocity = Vector3.zero;

            if (m_isMovingUp)
            {
                         

                velocity = Time.fixedDeltaTime * Vector3.up * (m_isSprinting ? m_movementData.SprintAcceleration : m_movementData.UpSpeed);
                velocity = Vector3.ClampMagnitude(velocity, m_movementData.UpSpeed);
            }
            else if (m_isMovingDown)
            {

                velocity = Time.fixedDeltaTime * Vector3.down * (m_isSprinting ? m_movementData.SprintAcceleration : m_movementData.DownSpeed);
                velocity = Vector3.ClampMagnitude(velocity, m_movementData.DownSpeed);
            }
        
            return velocity;
        }
        private void EnableInput()
        {
            m_inputHandler.DisableInput(false, true);
            m_isInputDisabled = false;
        }

        private void DisableInput()
        {            
            m_inputHandler.DisableInput(true, true);
            m_isInputDisabled = true;

        }
        private void OnVoidEnter()
        {
            DisableInput();
            m_voidIntro = true;
            m_timer = 0f;
        }

    }
}