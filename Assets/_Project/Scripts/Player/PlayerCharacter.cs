using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Sectors;
using System;

namespace OOB.Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private SectorTracker m_sectorTracker = default;
        [SerializeField] private PlayerInputHandler m_inputHandler = default;
        [SerializeField] private PlayerHealthComponent m_playerHealth = default;
        [SerializeField] private PlayerAttack m_playerAttack = default;
        [SerializeField] private CharacterWorldMovement m_worldMovement = default;
        [SerializeField] private VoidMovement m_voidMovement = default;
        [SerializeField] private Rigidbody m_rigidbody = default;
        [SerializeField] private CameraSwitch m_cameraSwitch = default;
        [SerializeField] private PlayerAnimationController m_playerAnimation = default;
        [SerializeField] private PlayerSoundController m_playerSound = default;
        [SerializeField] private AmbientSoundTrigger m_ambientTrigger;
        public bool IsInVoid() => m_voidMovement.enabled;

        public event Action OnPushingObjectStart;
        public event Action OnPushinObjectStop;
        public event Action OnPlayerIsInVoidStopAmbience;
        public event Action OnPlayIsInVoidStartAmbience;
        public event Action OnPlayerIsInVoidStop;
        public event Action OnPlayerIsInVoidStart;
        public event Action OnPlayerSpawn;
        public event Action OnPlayerDeath;
        public event Action OnStartGame;


        
        public SectorTracker GetSectorTracker() => m_sectorTracker;
        public PlayerInputHandler GetInputHandler() => m_inputHandler;
        public PlayerHealthComponent GetPlayerHealth() => m_playerHealth;
        public CharacterWorldMovement GetCharacterWorldMovement() => m_worldMovement;
        public VoidMovement GetVoidMovement() => m_voidMovement;
        public PlayerSoundController GetPlayerSoundController() => m_playerSound;
        public PlayerAnimationController GetPlayerAnimationController() => m_playerAnimation;
        public CameraSwitch GetCameraSwitch() => m_cameraSwitch;
        public bool GetIsPlayerDeath() => m_isPlayerDeath;
        private void SetPlayerNotDeath() => m_isPlayerReadyToStart = true;

        private CapsuleCollider m_collider;
        private bool m_statusQuo;
        private bool m_firstRun = true;
        private bool m_isPlayerDeath;
        private bool m_isPlayerReadyToStart;

        public void PushingObjectStart()
        {
            m_worldMovement.CanSprint(false);
            m_worldMovement.CanDash(false);
            OnPushingObjectStart?.Invoke();
        }

        public void PushingObjectStop()
        {
            OnPushinObjectStop?.Invoke();
            m_worldMovement.CanSprint(true);
            m_worldMovement.CanDash(true);
        }

        private void Awake() => Initialize();
        private void Initialize()
        {
            m_playerHealth.OnDeath += HandlePlayerDeath;

            m_collider = GetComponent<CapsuleCollider>();

            m_worldMovement.Initialize(m_rigidbody, m_collider, m_inputHandler, m_playerAttack, m_cameraSwitch, m_playerAnimation);
            m_voidMovement.Initialize(m_rigidbody, m_inputHandler);
            m_playerAttack.Initialize(m_worldMovement);
            
        }

        private void OnEnable()
        {
            m_sectorTracker.OnWorldEnter += HandleWorldEnter;
            m_sectorTracker.OnWorldExit += HandleWorldExit;
            m_playerAnimation.OnSwitchActivationEnded += EnableInput;
            m_playerAnimation.OnBeginSpinEnded += EnableInput;
            m_playerAnimation.OnBeginSpinEnded += SetPlayerNotDeath;
        }

        private void OnDisable()
        {
            m_sectorTracker.OnWorldEnter -= HandleWorldEnter;
            m_sectorTracker.OnWorldExit -= HandleWorldExit;
            m_rigidbody.velocity = Vector3.zero;
            m_playerAnimation.OnSwitchActivationEnded -= EnableInput;
            m_playerAnimation.OnBeginSpinEnded -= EnableInput;
            m_playerAnimation.OnBeginSpinEnded -= SetPlayerNotDeath;
        }

        private void HandleWorldEnter()
        {
            if(m_isPlayerReadyToStart)
                m_worldMovement.enabled = true;

            m_voidMovement.enabled = false;
            m_rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            if (m_voidMovement.GetIsInputDisabled() && m_isPlayerReadyToStart)
                m_inputHandler.DisableInput(false, false);


            StartCoroutine(IsPlayerChangedToWorld());
        }

        private void HandleWorldExit()
        {
            m_worldMovement.enabled = false;
            m_voidMovement.enabled = true;
            m_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            StartCoroutine(IsPlayerChangedToVoid());
        }

        private IEnumerator IsPlayerChangedToVoid()
        {
            yield return new WaitForSeconds(1);
            if (m_voidMovement.enabled)
            {
                if (m_ambientTrigger.m_WindCounter > 0)
                {
                    m_ambientTrigger.m_WindCounter = m_ambientTrigger.m_WindCounter - 2;
                } else
                {
                    OnPlayerIsInVoidStopAmbience?.Invoke();
                }
                


                m_statusQuo = true;
            }

            OnPlayerIsInVoidStart?.Invoke();
            yield return null;
        }

        private IEnumerator IsPlayerChangedToWorld()
        {
            yield return new WaitForSeconds(1);
            if (m_worldMovement.enabled && m_statusQuo)
            {

                if (m_ambientTrigger.m_WindCounter == 0)
                {
                    OnPlayIsInVoidStartAmbience?.Invoke();
                }

                m_statusQuo = false;
            }

            OnPlayerIsInVoidStop?.Invoke();
            yield return null;
        }



        private void Update()
        {
            UpdateMoveInput();

            if (m_isPlayerDeath)
                if (!m_playerAnimation.IsDeathAnimationPlaying())
                    ResetPlayer();

            if (m_firstRun)
            {
                OnPlayIsInVoidStartAmbience?.Invoke();
                OnStartGame?.Invoke();
                m_firstRun = false;
            }
                

        }


        private void UpdateMoveInput()
        {
            m_worldMovement.m_moveInput = m_inputHandler.MoveInput;
            m_voidMovement.MoveInput = m_inputHandler.MoveInput;
            PlayerCollision();
        }

        private void PlayerCollision()
        {
            bool isCollided = false;
            RaycastHit hit;
            isCollided |= Physics.Raycast(m_collider.bounds.center + Vector3.up * m_collider.height / 3, transform.forward, out hit, 1.0f);
            isCollided |= Physics.Raycast(m_collider.bounds.center + Vector3.up * m_collider.height / 2, transform.up, out hit, 1.0f);
            isCollided |= Physics.Raycast(m_collider.bounds.center + Vector3.down * m_collider.height / 5, transform.forward, out hit, 1.0f);

            if (isCollided)
            {
                m_worldMovement.CanSprint(false);
                m_voidMovement.CanSprint(false);
            }
            else
            {
                m_worldMovement.CanSprint(true);
                m_voidMovement.CanSprint(true);
            }
               
            
        }
        private void HandlePlayerDeath()
        {
           m_isPlayerDeath = true;
           m_isPlayerReadyToStart = false;
           m_inputHandler.DisableInput(true, false);
           OnPlayerDeath?.Invoke();
        }

        private void ResetPlayer()
        {
            gameObject.SetActive(false);
            m_sectorTracker.ResetTracker();
        }
        public void Revive()
        {
            m_inputHandler.DisableInput(true, false);
            m_playerHealth.SetToMaxHealth();
            gameObject.SetActive(true);
            OnPlayerSpawn?.Invoke();
            m_isPlayerDeath = false;
        }

        private void EnableInput()
        {
            m_inputHandler.DisableInput(false, false);
        }

        private void OnDestroy()
        {
        OnPushingObjectStart = null;
        OnPushinObjectStop = null;
        OnPlayerIsInVoidStopAmbience = null;
        OnPlayIsInVoidStartAmbience = null;
        OnPlayerIsInVoidStop = null;
        OnPlayerIsInVoidStart = null;
        OnPlayerSpawn = null;
        OnPlayerDeath = null;
        OnStartGame = null;
    }
    }
}