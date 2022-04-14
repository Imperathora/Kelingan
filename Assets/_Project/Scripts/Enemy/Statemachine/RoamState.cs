//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Character;
using Framework;

namespace OOB.Enemy.Statemachine
{
    public class RoamState : BaseState
    {
        public RoamState(MobileEnemy _enemy, Vector3 _center, float _roamRange) : base(_enemy.gameObject, typeof(IdleState))
        {
            m_enemy = _enemy;
            m_center = _center;
            m_roamRange = _roamRange;
        }

        private MobileEnemy m_enemy;
        private Vector3 m_center;
        private float m_roamRange;

        private float MinDestDistance => m_enemy.RoamMoveBalancing.MinimumDestinationDistance;

        private float MinRandomDistance => m_enemy.RoamBalancing.MinRandomDistance;
        private float MaxRandomDistance => m_enemy.RoamBalancing.MaxRandomDistance;

        private Vector3 m_destination;

        public override void FixedUpdate() => Roam();
        private void Roam()
        {
            float angleDifference = Vector3.Angle(m_destination - transform.position, transform.forward);

            TurnToDestination(angleDifference);

            MoveToDestination(angleDifference);

            if (Vector3.Distance(transform.position, m_destination) < MinDestDistance)
                StateFinished(NextState);
        }    

        private void TurnToDestination(float _angleDiff)
        {
            float turnSpeed = m_enemy.GetRoamTurnSpeed(_angleDiff);
            transform.TurnTowards(m_destination, Time.fixedDeltaTime * turnSpeed);
        }

        private void MoveToDestination(float _angleDiff)
        {
            float moveSpeed = m_enemy.GetRoamMoveSpeed(_angleDiff);
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * moveSpeed);
        }

        private Vector3 GetRandomDestination()
        {
            Vector3 pos = transform.position;

            int i = 0;
            while (Vector3.Distance(pos, transform.position) < MinRandomDistance
                || Vector3.Distance(pos, transform.position) > MaxRandomDistance)
            {
                i++;
                if (i > 100)
                {
                    pos = m_center;
                    break;
                }

                float x = Random.Range(-1f, 1f);
                float z = Random.Range(-1f, 1f);

                Vector2 vec = new Vector2(x, z);
                vec.Normalize();
                vec *= Random.Range(0f, 1f);
                vec *= m_roamRange;
                pos.Set(m_center.x + vec.x, m_center.y, m_center.z + vec.y);
            }

            return pos;
        }

        public override void EnterState()
        {
            m_destination = GetRandomDestination();
        }
    }
}