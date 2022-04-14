using OOB.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Enemy.Statemachine;

namespace OOB.Enemy
{
    public class StaticEnemy : EnemyCharacter
    {
        [Header("Track Balancing")]
        [SerializeField] private EnemyTurnBalancingData m_trackTurnBalancing = default;
        public EnemyTurnBalancingData TrackTurnBalancing => m_trackTurnBalancing;
        [SerializeField] private EnemyAttackBalancingData m_trackAttackBalancing = default;
        public EnemyAttackBalancingData TrackAttackBalancing => m_trackAttackBalancing;

        public float GetTrackTurnSpeed(float _angleDifference) => GetAdjustedTurnSpeed(_angleDifference, m_trackTurnBalancing);

        public void Initialize()
        {
            Transform playerTransform = WorldComponent.Instance?.GetPlayerCharacter()?.transform;

            Dictionary<System.Type, BaseState> states = new Dictionary<System.Type, BaseState>
            {
                {typeof(IdleState), new IdleState(this, typeof(IdleState)) },
                {typeof(TrackState), new TrackState(this, playerTransform) }
            };

            m_statemachine = new EnemyStateMachine();
            m_statemachine.Initialize(states, typeof(IdleState));
        }

        protected override void HandlePlayerEnter()
        {
            m_attack.StartAttacking();
            m_statemachine.SwitchToNewState(typeof(TrackState));
        }

        private void FixedUpdate()
        {
            m_statemachine.FixedUpdate();
        }
    }
}