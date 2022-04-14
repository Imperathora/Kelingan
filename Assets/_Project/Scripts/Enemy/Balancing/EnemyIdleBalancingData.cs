using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    [CreateAssetMenu(fileName = "Idle Balancing Data", menuName = "Balancing/Idle Balancing Data")]
    public class EnemyIdleBalancingData : ScriptableObject
    {
        // ===== Random Behaviour =====

        [Header("Random Behaviour")]
        [Range(0f, 180f)]
        [SerializeField] private float m_minRandomAngle = default;
        [Range(0f, 180f)]
        [SerializeField] private float m_maxRandomAngle = default;
        [SerializeField] private float m_minRandomWaitDuration = default;
        [SerializeField] private float m_maxRandomWaitDuration = default;


        // ===== Properties =====

        public float MinRandomAngle => m_minRandomAngle;
        public float MaxRandomAngle => m_maxRandomAngle;
        public float MinRandomWaitDuration => m_minRandomWaitDuration;
        public float MaxRandomWaitDuration => m_maxRandomWaitDuration;
    }
}