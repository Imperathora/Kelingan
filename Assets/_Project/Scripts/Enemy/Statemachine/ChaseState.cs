using OOB.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace OOB.Enemy.Statemachine
{
    public class ChaseState : BaseState
    {
        public ChaseState(MobileEnemy _enemy, Transform _player) : base(_enemy.gameObject, typeof(IdleState))
        {
            m_enemy = _enemy;
            Player = _player;
        }

        private MobileEnemy m_enemy;

        private float MinDestDistance => m_enemy.ChaseMoveBalancing.MinimumDestinationDistance;

        private Transform Player { get; }
        private Vector3 m_destination;

        public override void FixedUpdate() => Chase();
        private void Chase()
        {
            m_destination = Player.position;

            float angleDifference = Vector3.Angle(m_destination - transform.position, transform.forward);

            float turnSpeed = m_enemy.GetChaseTurnSpeed(angleDifference);
            transform.TurnTowards(m_destination, turnSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, m_destination) > MinDestDistance)
            {
                float moveSpeed = m_enemy.GetChaseMoveSpeed(angleDifference);
                transform.Translate(Vector3.forward * Time.fixedDeltaTime * moveSpeed);
            }
        }
    }
}