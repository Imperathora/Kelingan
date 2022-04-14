using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using OOB.Enemy.Statemachine;

namespace OOB.Enemy
{
    public class MobileEnemy : EnemyCharacter
    {
        [Header("Roam Balancing")]
        [SerializeField] private EnemyRoamBalancingData m_roamBalancing = default;
        [SerializeField] private EnemyMoveBalancingData m_roamMoveBalancing = default;
        [SerializeField] private EnemyTurnBalancingData m_roamTurnBalancing = default;
        public EnemyRoamBalancingData RoamBalancing => m_roamBalancing;
        public EnemyMoveBalancingData RoamMoveBalancing => m_roamMoveBalancing;
        public EnemyTurnBalancingData RoamTurnBalancing => m_roamTurnBalancing;

        public float GetRoamMoveSpeed(float _angleDifference) => GetAdjustedMoveSpeed(_angleDifference, m_roamMoveBalancing);
        public float GetRoamTurnSpeed(float _angleDifference) => GetAdjustedTurnSpeed(_angleDifference, m_roamTurnBalancing);

        [Header("Chase Balancing")]
        [SerializeField] private EnemyMoveBalancingData m_chaseMoveBalancing = default;
        [SerializeField] private EnemyTurnBalancingData m_chaseTurnBalancing = default;
        [SerializeField] private EnemyAttackBalancingData m_chaseAttackBalancing = default;
        public EnemyMoveBalancingData ChaseMoveBalancing => m_chaseMoveBalancing;
        public EnemyTurnBalancingData ChaseTurnBalancing => m_chaseTurnBalancing;
        public EnemyAttackBalancingData ChaseAttackBalancing => m_chaseAttackBalancing;

        public float GetChaseMoveSpeed(float _angleDifference) => GetAdjustedMoveSpeed(_angleDifference, m_chaseMoveBalancing);
        public float GetChaseTurnSpeed(float _angleDifference) => GetAdjustedTurnSpeed(_angleDifference, m_chaseTurnBalancing);

        public event System.Action OnConfinementLeft;
        public event System.Action OnStartAttacking;

        public Vector3 ConfinementCenter { get; private set; }
        public float RoamRange { get; private set; }
        public float ConfinementRadius { get; private set; }

        public void Initialize(Vector3 _confinementCenter, float _maxConfinementRadius, float _maxRoamRange)
        {
            Transform playerTransform = WorldComponent.Instance?.GetPlayerCharacter()?.transform;

            ConfinementCenter = _confinementCenter;
            ConfinementRadius = _maxConfinementRadius;
            RoamRange = _maxRoamRange;

            Dictionary<System.Type, BaseState> states = new Dictionary<System.Type, BaseState>
            {
                {typeof(IdleState), new IdleState(this, typeof(RoamState)) },
                {typeof(RoamState), new RoamState(this, ConfinementCenter, RoamRange) },
                {typeof(ChaseState), new ChaseState(this, playerTransform) }
            };

            m_statemachine = new EnemyStateMachine();
            m_statemachine.Initialize(states, typeof(IdleState));
        }

        protected override void HandlePlayerEnter()
        {
            m_attack.StartAttacking();
            m_statemachine.SwitchToNewState(typeof(ChaseState));
            OnStartAttacking?.Invoke();
        }

        private void FixedUpdate()
        {
            m_statemachine.FixedUpdate();

            CheckConfinement();
        }

        private void CheckConfinement()
        {
            if (HasLeftConfinement())
            {
                gameObject.SetActive(false);
                OnConfinementLeft?.Invoke();
            }
        }

        private bool HasLeftConfinement()
        {
            return Vector3.Distance(transform.position, ConfinementCenter) > ConfinementRadius;
        }

        private void OnDestroy()
        {
            OnConfinementLeft = null;
        }
    }
}