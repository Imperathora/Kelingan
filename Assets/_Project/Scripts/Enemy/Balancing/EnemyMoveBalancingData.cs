using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Move Balancing Data", menuName = "Balancing/Enemy Move Balancing Data")]
    public class EnemyMoveBalancingData : ScriptableObject
    {
        // ===== Move Speed Values =====

        [Header("Movement Speed")]
        [Tooltip("Between minimum and maximum angle to raget the speed will lerp between maximum and minimum.")]
        [SerializeField] private float m_minMoveSpeed = default;

        [Tooltip("Between minimum and maximum angle to raget the speed will lerp between maximum and minimum.")]
        [SerializeField] private float m_maxMoveSpeed = default;


        [Range(0f, 180f)]
        [Tooltip("Below this angle towards target the enemy will always move at maximum speed.\n" +
            "Has to be less than MaxMoveAngle.")]
        [SerializeField] private float m_minMoveAngle = default;

        [Range(0f, 180f)]
        [Tooltip("Above this angle towards target the enemy will always move at minimum speed.\n" +
            "Has to be greater than MinMoveAngle.")]
        [SerializeField] private float m_maxMoveAngle = default;
                       

        // ===== Minimum Destination Distance =====

        [Header("Destination Distance")]
        [Tooltip("The enemy won't get closer to its target than this.")]
        [SerializeField] private float m_minimumDestinationDistance = default;


        // ===== Properties =====

        public float MinMoveSpeed => m_minMoveSpeed;
        public float MaxMoveSpeed => m_maxMoveSpeed;
        public float MinMoveAngle => m_minMoveAngle;
        public float MaxMoveAngle => m_maxMoveAngle;

        public float MinimumDestinationDistance => m_minimumDestinationDistance;
    }
}