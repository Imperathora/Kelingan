using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OOB.Collectible;
using TMPro;

namespace OOB.UI
{
    public class CollectibleDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text = default;
        [SerializeField] private Type m_collectibleType = default;
        [SerializeField] private float m_waitTimerForAnimation = default;
        private Animator m_animator;

        public static event System.Action OnReverseCoin;
        public static event System.Action OnReverseStar;

        public static event System.Action OnFlyInCoin;
        public static event System.Action OnFlyInStar;

        private bool m_coinPickedUp = false;
        private bool m_coinTimerOnPickUp = false;
        private bool m_StarTimerOnPickUp = false;
        private bool m_flyIn = false;

        private float m_timerCoin;
        private float m_timerStar;
        private float m_resetTimeCoin;
        private float m_resetTimeStar;

        private void Start()
        {
            m_timerCoin = m_waitTimerForAnimation;
            m_resetTimeCoin = m_timerCoin;

            m_timerStar = m_waitTimerForAnimation;
            m_resetTimeStar = m_timerStar;

            CollectibleCounter counter = GameManager.Instance?.GetCollectibleCounter();
            if (counter == null)
            {
                Debug.LogWarning("No collectible counter found!", this);
                return;
            }
       
            m_animator = GetComponent<Animator>();

            if (m_animator == null)
                return;

            if (m_collectibleType == Type.Coin)
            {
                counter.OnCoinCounterUpdated += UpdateText;
            }
            else
            {
                counter.OnStarCounterUpdated += UpdateText;
            }
        }

        private void Update()
        {
            switch (m_collectibleType)
            {
                case Type.Coin:
                    if (m_coinTimerOnPickUp)
                    {
                        m_timerCoin -= Time.deltaTime;

                        if (m_timerCoin < 0)
                        {
                            m_timerCoin = 0;
                            ResetTimer();
                        }
                    }
                    break;

                case Type.Star:
                    if (m_StarTimerOnPickUp)
                    {
                        m_timerStar -= Time.deltaTime;

                        if (m_timerStar < 0)
                        {
                            m_timerStar = 0;
                            ResetTimer();
                        }
                    }
                    break;
            }
        }

        private void ResetTimer()
        {
            switch (m_collectibleType)
            {
                case Type.Coin:
                    if (m_timerCoin <= 0)
                    {
                        m_coinTimerOnPickUp = false;
                        m_timerCoin = m_resetTimeCoin;
                        m_coinPickedUp = true;
                    }

                    if (m_coinPickedUp && !m_flyIn)
                    {
                        m_animator.SetFloat("speedMultiplier", 1);
                        m_coinPickedUp = false;
                        AnimationReverse();
                    }
                    break;

                case Type.Star:

                    if (m_timerStar <= 0)
                    {
                        m_StarTimerOnPickUp = false;
                        m_timerStar = m_resetTimeStar;
                        AnimationReverse();
                    }
                    break;
            }
        }

        bool AnimatorIsPlaying()
        {
            return m_animator.GetCurrentAnimatorStateInfo(0).length >
                   m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        bool AnimatorIsPlayingThis(string stateName)
        {
            return AnimatorIsPlaying() && m_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        private void AnimationReverse()
        {
            switch (m_collectibleType)
            {
                case Type.Coin:
                    OnReverseCoin.Invoke();
                    break;

                case Type.Star:
                    OnReverseStar.Invoke();
                    break;
            }
        }

        private void UpdateText(int _newCount)
        {
            switch (m_collectibleType)
            {
                case Type.Coin:
                    m_timerCoin = m_resetTimeCoin;
                    m_coinTimerOnPickUp = true;

                    if (AnimatorIsPlayingThis("CoinUIReverse"))
                    {
                        m_animator.SetFloat("speedMultiplier", -1);
                    }

                    else
                    {
                        OnFlyInCoin.Invoke();
                    }

                    m_text.text = _newCount.ToString();
                    break;

                case Type.Star:
                    m_timerStar = m_resetTimeStar;
                    m_StarTimerOnPickUp = true;
                    OnFlyInStar.Invoke();
                    m_text.text = _newCount.ToString();
                    break;
            }
        }

        private void OnDestroy()
        {
            CollectibleCounter counter = GameManager.Instance?.GetCollectibleCounter();
            if (counter == null)
            {
                return;
            }

            if (m_collectibleType == Type.Coin)
            {
                counter.OnCoinCounterUpdated -= UpdateText;
            }
            else
            {
                counter.OnStarCounterUpdated -= UpdateText;
            }

            counter.ResetCounter(Type.Coin);
            counter.ResetCounter(Type.Star);
        }
    }
}