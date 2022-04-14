using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Player
{
    [CreateAssetMenu(fileName = "Player Attack Balaning", menuName = "Balancing/Player Attack")]
    public class PlayerAttackBalancingData : ScriptableObject
    {
        [SerializeField] private int m_damage = default;

        public int Damage => m_damage;
    }
}