using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Player
{
    [CreateAssetMenu(fileName = "Player Spawn Balancing Data", menuName = "Balancing/Player Spawn Balancing Data")]
    public class PlayerSpawnBalancingData : ScriptableObject
    {
        [SerializeField] private float m_respawnDelay = default;

        public float RespawnDelay => m_respawnDelay;
    }
}