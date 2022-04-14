using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using Framework.Data;
using System;
using OOB.Player;

namespace OOB.Sectors
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private bool m_isReactingToSectorActivation = default;
        [SerializeField] private Material m_PortalMaterialOn = null;
        [SerializeField] private UnityEvent OnPlayerEnteredPortal = default;
        [SerializeField] private WorldBalancingData m_worldBalancing = default;

        public event Action OnEnterPortal;
        private Renderer m_PortalRenderer = null;

        private bool m_areAllSectorsActivated;

        private void Awake()
        {
            m_PortalRenderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerCharacter playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();

            if (!m_isReactingToSectorActivation || m_areAllSectorsActivated)
            {

                if (other.transform == playerCharacter.transform)
                {
                    OnEnterPortal?.Invoke();
                    StartCoroutine(WaitBeforeNextLevel(m_worldBalancing.DelayTime, playerCharacter));
                }
                    
            }
                
        }

        void Start()
        {
            SectorRegistry registry = WorldComponent.Instance?.GetSectorRegistry();
            if (registry == null)
            {
                Debug.LogError("Registry wasn't found", this);
                return;
            }
            registry.OnAllSectorsActivated += HandleAllSectorsActivated;

        }

        private void HandleAllSectorsActivated()
        {
            m_areAllSectorsActivated = true;
            m_PortalRenderer.material = m_PortalMaterialOn;
        }

        private void OnDestroy()
        {
            SectorRegistry registry = WorldComponent.Instance?.GetSectorRegistry();
            if (registry != null)
                registry.OnAllSectorsActivated -= HandleAllSectorsActivated;

            OnEnterPortal = null;
             
        }

        private IEnumerator WaitBeforeNextLevel(float _time, PlayerCharacter _player)
        {
            _player.GetPlayerSoundController()?.PlayPortalSound();
            _player.GetInputHandler().DisableInput(true, false);
            SoundController.Instance?.StopAll();
            yield return new WaitForSeconds(_time);
            OnPlayerEnteredPortal.Invoke();
        }
    }
}