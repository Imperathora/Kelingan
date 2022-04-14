using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Player
{
    [RequireComponent(typeof(Collider))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Collider m_collider = default;
        [SerializeField] private PlayerAttackBalancingData m_attackBalancing = default;

        public event System.Action OnEnemyHit;

        public void Initialize(CharacterWorldMovement _movement)
        {
            _movement.OnDashStarted += ActivateTrigger;
            _movement.OnDashFinished += DeactivateTrigger;
        }

        private void ActivateTrigger() => m_collider.enabled = true;
        private void DeactivateTrigger() => m_collider.enabled = false;

        private void OnTriggerEnter(Collider other)
        {
            Character.IDamageable target = other.GetComponent<Character.IDamageable>();
            if (target != null)
            {
                target.TakeDamage(m_attackBalancing.Damage);
                OnEnemyHit?.Invoke();
            }
        }

        private void OnDestroy() => OnEnemyHit = null;
    }
}