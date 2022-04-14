using OOB.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy.Statemachine
{
    public class IdleState : BaseState
    {
        public IdleState(EnemyCharacter _enemy, System.Type _nextState) : base(_enemy.gameObject, _nextState)
        {
            m_enemy = _enemy;
        }

        private EnemyCharacter m_enemy;

        private float MinRandomWaitDuration => m_enemy.IdleBalancing.MinRandomWaitDuration;
        private float MaxRandomWaitDuration => m_enemy.IdleBalancing.MaxRandomWaitDuration;

        private Vector3? m_targetDirection;
        private float m_stateDuration;

        private float m_timer;

        public override void FixedUpdate()
        {
            m_timer += Time.fixedDeltaTime;
            if (m_timer >= m_stateDuration)
                StateFinished(NextState);
        }

        public override void EnterState()
        {
            m_stateDuration = Random.Range(MinRandomWaitDuration, MaxRandomWaitDuration);
        }

        public override void ExitState()
        {
            m_timer = 0f;
        }
    }
}