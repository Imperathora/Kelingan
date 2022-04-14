using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Character
{
    public abstract class HealthComponent : MonoBehaviour, IKillable, IDamageable
    {
        /// <summary>
        /// int = CurrentHealth, float = PercentHealth
        /// </summary>
        public event System.Action<int, float> OnHealthChanged;

        public event System.Action OnDeath;
        public static System.Action OnFadeOut;


        public abstract int MaxHealth { get; }

        protected int m_currentHealth;
        public int CurrentHealth
        {
            get { return m_currentHealth; }
            protected set
            {
                m_currentHealth = value;
                OnHealthChanged?.Invoke(m_currentHealth, PercentHealth());
            }
        }

        public abstract void TakeDamage(int _damageTaken);
        public virtual void Die()
        {
            OnDeath?.Invoke();
        }

        public abstract void Heal();

        public abstract void FullHeal();

        public abstract void TakeFloorDamage();

        public float PercentHealth()
        {
            return (float)m_currentHealth / (float)MaxHealth;
        }

        public void SetToMaxHealth()
        {
            m_currentHealth = MaxHealth;
            OnHealthChanged?.Invoke(m_currentHealth, PercentHealth());
        }

        private void TakeDamage(int _damageTaken, out int _damageDealt)
        {
            _damageDealt = _damageTaken;
            TakeDamage(_damageTaken);
        }

        protected void OnDestroy() => OnHealthChanged = null;
    }
}