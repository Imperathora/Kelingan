using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Enemy.Statemachine;

namespace OOB.Enemy
{
    public abstract class EnemyCharacter : MonoBehaviour
    {
        [SerializeField] private Character.HealthComponent m_healthComponent = default;
        public Character.HealthComponent HealthComponent() => m_healthComponent;

        [SerializeField] protected PlayerTracker m_playerTracker = default;
        [SerializeField] protected EnemyAttack m_attack = default;

        [Header("Idle Balancing")]
        [SerializeField] private EnemyIdleBalancingData m_idleBalancing = default;
        [SerializeField] private EnemyTurnBalancingData m_idleTurnBalancing = default;
        public EnemyIdleBalancingData IdleBalancing => m_idleBalancing;
        public EnemyTurnBalancingData IdleTurnBalancing => m_idleTurnBalancing;
        
        protected EnemyStateMachine m_statemachine;

        protected virtual void Awake()
        {
            m_healthComponent.OnDeath += HandleDeath;
            m_playerTracker.OnPlayerEnter += HandlePlayerEnter;
            m_playerTracker.OnPlayerExit += HandlePlayerExit;
        }

        public float GetAdjustedTurnSpeed(float _angleDiff, EnemyTurnBalancingData _turnBalancing)
        {
            return Framework.Utils.LerpMinMax(
                _turnBalancing.MinTurnSpeed, _turnBalancing.MaxTurnSpeed,
                _angleDiff,
                _turnBalancing.MinTurnAngle, _turnBalancing.MaxTurnAngle);
        }

        public float GetAdjustedMoveSpeed(float _angleDiff, EnemyMoveBalancingData _movementBalancing)
        {
            return Framework.Utils.LerpMinMax(
                _movementBalancing.MaxMoveSpeed, _movementBalancing.MinMoveSpeed, 
                _angleDiff,
                _movementBalancing.MinMoveAngle, _movementBalancing.MaxMoveAngle);
        }

        protected abstract void HandlePlayerEnter();

        private void HandlePlayerExit()
        {
            m_attack.StopAttacking();
            m_statemachine.SwitchToNewState(typeof(IdleState));
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        public void Revive()
        {
            m_healthComponent.SetToMaxHealth();
            gameObject.SetActive(true);
        }
    }
}