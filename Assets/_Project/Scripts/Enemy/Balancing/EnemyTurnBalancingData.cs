using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Turn Balancing Data", menuName = "Balancing/Enemy Turn Balancing Data")]
    public class EnemyTurnBalancingData : ScriptableObject
    {
        // ===== Turn Speed Values =====

        [Header("Turn Speed")]
        [Tooltip("Between minimum and maximum angle to raget the speed will lerp between minimum and maximum.")]
        [SerializeField] private float m_minTurnSpeed = default;

        [Tooltip("Between minimum and maximum angle to raget the speed will lerp between minimum and maximum.")]
        [SerializeField] private float m_maxTurnSpeed = default;


        [Range(0f, 180f)]
        [Tooltip("Below this angle towards target the enemy will always turn at minimum speed.\n" +
            "Has to be less than MaxTurnAngle.")]
        [SerializeField] private float m_minTurnAngle = default;

        [Range(0f, 180f)]
        [Tooltip("Above this angle towards target the enemy will always turn at maximum speed.\n" +
            "Has to be greater than MinTurnAngle.")]
        [SerializeField] private float m_maxTurnAngle = default;


        // ===== Properties =====

        public float MinTurnSpeed => m_minTurnSpeed;
        public float MaxTurnSpeed => m_maxTurnSpeed;
        public float MinTurnAngle => m_minTurnAngle;
        public float MaxTurnAngle => m_maxTurnAngle;
    }
}