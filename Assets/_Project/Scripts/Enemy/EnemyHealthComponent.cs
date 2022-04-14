using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OOB.Enemy
{
    public class EnemyHealthComponent : Character.HealthComponent
    {
        [SerializeField] private EnemyHealthBalancingData m_healthBalancing = default;

        public UnityEvent OnDamageTaken;
        public UnityEvent OnDeath_External;

        public override int MaxHealth => m_healthBalancing.MaxHealth;

        private void Awake()
        {
            SetToMaxHealth();
        }

        public override void Heal()
        {
            return;
        }

        public override void FullHeal()
        {
            return;
        }

        public override void TakeFloorDamage()
        {
            return;
        }

        public override void Die()
        {
            OnFadeOut.Invoke();
            base.Die();
            OnDeath_External.Invoke();
        }

        public override void TakeDamage(int _damageTaken)
        {
            if (_damageTaken > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
            {
                CurrentHealth -= _damageTaken;
            }

            if (CurrentHealth <= 0)
                Die();
        }
    }
}