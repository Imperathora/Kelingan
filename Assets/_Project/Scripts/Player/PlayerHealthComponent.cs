using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OOB.Character;
using OOB.Collectible;
using System;

namespace OOB.Player
{
    public class PlayerHealthComponent : HealthComponent
    {
        [SerializeField] private PlayerHealthBalancingData m_healthBalancing = default;
        public override int MaxHealth { get { return m_healthBalancing.MaxHealth; } }
        public float ImmunityDuration { get { return m_healthBalancing.ImmunityDuration; } }

        public event System.Action<float> OnHealTriggered;
        public event System.Action OnMaxTriggered;

        //public UnityEvent OnDamageTaken;
        public new event System.Action OnDeath;

        public event Action OnDamageTaken;


        CollectibleCounter m_counter;
        //public new UnityEvent OnDeath;

        private void Awake()
        {
            SetToMaxHealth();
        }

        private void Start()
        {
            m_counter = GameManager.Instance?.GetCollectibleCounter();
            if (m_counter == null)
            {
                Debug.LogWarning("No collectible counter found!", this);
                return;
            }

            m_counter.OnCoinCollected += Heal;
            m_counter.OnStarCollected += FullHeal;
            PlayerFloorDamage.OnDamageMaterial += TakeFloorDamage;
        }

        public override void TakeFloorDamage()
        {
            PlayerCharacter player = GetComponentInParent<PlayerCharacter>();

            if (CurrentHealth > 0)
            {
               CurrentHealth = CurrentHealth -1;
                OnDamageTaken?.Invoke();
            }

            if (CurrentHealth <= 0 && !player.GetIsPlayerDeath())
                Die();
        }


        public override void TakeDamage(int _damageTaken)
        {

            PlayerCharacter player = GetComponentInParent<PlayerCharacter>();

            if (_damageTaken > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
            {
                CurrentHealth -= _damageTaken;
                OnDamageTaken?.Invoke();
            }

            if (CurrentHealth <= 0 && !player.GetIsPlayerDeath())
                Die();
        }

        public override void FullHeal()
        {
            if (CurrentHealth != MaxHealth)
            {
                CurrentHealth = MaxHealth;
                OnMaxTriggered?.Invoke();
            }
        }

        public override void Heal()
        {
            if (CurrentHealth != MaxHealth)
            {
                CurrentHealth += 1;
                OnHealTriggered?.Invoke(PercentHealth());
                if(CurrentHealth == MaxHealth)
                    OnMaxTriggered?.Invoke();
            }
        }

        public override void Die()
        {
            OnDeath?.Invoke();
        }

        private new void OnDestroy()
        {
            m_counter = GameManager.Instance?.GetCollectibleCounter();
            if (m_counter == null)
            { 
                return;
            }
            m_counter.OnCoinCollected -= Heal;

            PlayerFloorDamage.OnDamageMaterial -= TakeFloorDamage;
        }
    }
}