using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Framework;
using OOB.Character;
using System;

namespace OOB.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private SphereCollider m_sphereTrigger = default;
        [SerializeField] private EnemyAttackBalancingData m_attackBalancing = default;

        [SerializeField] private UnityEvent OnAttackStarted = default;

        private bool m_isAttacking;

        private System.Action OnAttackFinished;


        public static System.Action OnDamageDealing;
        public static System.Action OnStopAttack;

        private WaitForSeconds m_delayTimer;
        private WaitForSeconds m_cooldownTimer;

        private void Awake()
        {
            m_delayTimer = new WaitForSeconds(m_attackBalancing.StartDelay);
            m_cooldownTimer = new WaitForSeconds(m_attackBalancing.Cooldown);
        }

        private void StartAttack()
        {
            StartCoroutine(AttackRoutine());
            OnAttackStarted.Invoke();
            
        }

        IEnumerator AttackRoutine()
        {
            yield return m_delayTimer;

            DealDamage();

            yield return m_cooldownTimer;

            OnAttackFinished?.Invoke();
        }

        private void DealDamage()
        {
            Collider coll = m_sphereTrigger.GetOverlappingCollider();

            if (coll != null)
            {
                IDamageable target = coll.GetComponent<IDamageable>();
                if (target == null)
                {
                    //Debug.LogError($"Target is not damageable ({coll.name}).", this);
                    return;
                }

                target.TakeDamage(m_attackBalancing.DamageAmount);
                OnDamageDealing.Invoke();
                //Debug.Log($"Dealing damage to {target}.", this);
            }

        }

        public void StartAttacking()
        {
            if (!m_isAttacking)
            {
                
                m_isAttacking = true;
                StartAttack();
                OnAttackFinished += StartAttack;
            }
        }

        public void StopAttacking()
        {
            if (m_isAttacking)
            {
                OnStopAttack.Invoke();
                   m_isAttacking = false;
                OnAttackFinished -= StartAttack;
            }
        }
    }
}