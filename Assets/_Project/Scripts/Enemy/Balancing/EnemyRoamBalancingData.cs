using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    [CreateAssetMenu(fileName = "Roam Balancing Data", menuName = "Balancing/Roam Balancing Data")]
    public class EnemyRoamBalancingData : ScriptableObject
    {        
        // ===== Random Behaviour =====

        [Header("Random Behaviour")]
        [SerializeField] private float m_minRandomDistance = default;
        [SerializeField] private float m_maxRandomDistance = default;


        // ===== Properties =====

        public float MinRandomDistance => m_minRandomDistance;
        public float MaxRandomDistance => m_maxRandomDistance;
    }
}