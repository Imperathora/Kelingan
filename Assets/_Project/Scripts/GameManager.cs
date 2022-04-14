using UnityEngine;
using Rewired;
using Framework.Input;
using System;

using OOB.Collectible;

namespace OOB
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private InputHandler[] m_inputHandlers = default;

        public event Action OnInitialization;

        private CollectibleCounter m_collectibleCounter = new CollectibleCounter();
        

        public CollectibleCounter GetCollectibleCounter()
        { return m_collectibleCounter; }

        private void Awake()
        {
            if (Instance == null)
            {
                 
                DontDestroyOnLoad(gameObject);
                Instance = this;
               
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
            InitializeInputHandlers();

            OnInitialization?.Invoke();
        }

        private void InitializeInputHandlers()
        {
            Rewired.Player rewiredPlayer = ReInput.players.GetPlayer(0);

            foreach (InputHandler handler in m_inputHandlers)
            {
                handler.Initialize(rewiredPlayer);
                handler.EnableInput(this);
            }
        }

    }
}