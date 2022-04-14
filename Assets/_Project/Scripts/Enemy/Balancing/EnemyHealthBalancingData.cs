using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Health Balancing Data", menuName = "Balancing/Enemy Health Balancing Data")]
    public class EnemyHealthBalancingData : ScriptableObject
    {
        [SerializeField] private int m_maxHealth = default;

        public int MaxHealth => m_maxHealth;
    }
}