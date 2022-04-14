using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace OOB.UI
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        //[SerializeField] private UnityEngine.UI.Text m_text = default;
        [SerializeField] private Image HealthBar = default;
        private Animator m_animator;

        public static event Action OnReverse;
        public static event Action OnFlyIn;

        private void Start()
        {
            WorldComponent.Instance.OnInitialization += Initialize;
            m_animator = GetComponent<Animator>();
        }

        private void Initialize()
        {
            Player.PlayerHealthComponent health = WorldComponent.Instance?.GetPlayerCharacter()?.GetPlayerHealth();
            if (health == null)
            {
                Debug.LogWarning("No PlayerHealth found!", this);
                return;
            }
            health.OnHealthChanged += UpdateHealthDamage;
        }

        private void UpdateHealthDamage(int _newHealth, float _percentHealth)
        {
            HealthBar.fillAmount = _percentHealth;
            m_animator.SetBool("isMax", _percentHealth == 1);
        }

        private void OnDestroy()
        {
            Player.PlayerHealthComponent health = WorldComponent.Instance?.GetPlayerCharacter()?.GetPlayerHealth();
            if (health == null)
            {
                return;
            }
            health.OnHealthChanged -= UpdateHealthDamage;
        }
    }
}