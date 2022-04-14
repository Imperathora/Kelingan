using OOB.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace OOB.Enemy.Statemachine
{
    public class TrackState : BaseState
    {
        public TrackState(StaticEnemy _enemy, Transform _player) : base(_enemy.gameObject, typeof(IdleState))
        {
            m_enemy = _enemy;
            Player = _player;
        }

        private StaticEnemy m_enemy;

        private Transform Player { get; }
        private Vector3 m_destination;

        public override void FixedUpdate()
        {
            m_destination = Player.position;

            float angleDifference = Vector3.Angle(m_destination - transform.position, transform.forward);

            float turnSpeed = m_enemy.GetTrackTurnSpeed(angleDifference);
            transform.TurnTowards(m_destination, turnSpeed * Time.fixedDeltaTime);
        }
    }
}