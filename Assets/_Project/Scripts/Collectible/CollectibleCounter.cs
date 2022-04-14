using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Collectible
{
    public class CollectibleCounter
    {
        public event Action<int> OnCoinCounterUpdated;
        public event Action<int> OnStarCounterUpdated;
        public event Action OnStarCollected;

        public event Action OnCoinCollected;

        private int m_coinCounter;

        public int CoinCounter
        {
            get { return m_coinCounter; }

            private set
            {
                m_coinCounter = value;
                OnCoinCounterUpdated?.Invoke(m_coinCounter);
            }
        }

        private int m_starCounter;
        public int StarCounter
        {
            get { return m_starCounter; }
            private set
            {
                m_starCounter = value;
                OnStarCounterUpdated?.Invoke(m_starCounter);
                OnStarCollected?.Invoke();
            }
        }

        public void Collect(Type _type)
        {
            if (_type == Type.Coin)
            {
                CoinCounter++;
                OnCoinCollected.Invoke();
            }
            else
            {
                StarCounter++;
            }
        }

        public void ResetCounter(Type _type)
        {
            if (_type == Type.Coin)
            {
                CoinCounter = 0;
            }
            else
            {
                StarCounter = 0;
            }
        }
    }
}