using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Sectors;
using OOB.Player;

namespace OOB
{
    public class WorldComponent : MonoBehaviour
    {
        public static WorldComponent Instance { get; private set; }

        public event Action OnInitialization;

        private SectorRegistry m_sectorRegistry;
        public SectorRegistry GetSectorRegistry() { return m_sectorRegistry; }

        private PlayerSpawner m_playerSpawner;

        private PlayerCharacter m_playerCharacter;
        public PlayerCharacter GetPlayerCharacter() { return m_playerCharacter; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                m_sectorRegistry = new SectorRegistry();
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_playerSpawner = GetComponentInChildren<PlayerSpawner>();

            StartCoroutine(InitializeAsync());       
        }

        private IEnumerator InitializeAsync()
        {
            yield return new WaitForEndOfFrame();

            m_sectorRegistry.Initialize();

            m_playerSpawner.Initialize(out m_playerCharacter);

            OnInitialization?.Invoke();
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }

        }

        private void OnDestroy()
        {
            m_sectorRegistry = null;
        }
    }
}