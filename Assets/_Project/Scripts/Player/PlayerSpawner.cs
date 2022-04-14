using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Player;

namespace OOB
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] GameObject m_playerPrefab = default;
        [SerializeField] PlayerSpawnBalancingData m_spawnBalancing = default;


        //public void Spawn()
        //{
        //    PlayerCharacter player = WorldComponent.Instance?.GetPlayerCharacter();
        //    if (player == null)
        //    {
        //        Debug.LogError("Player spawn failed!", this);
        //        return;
        //    }
        //}

        public void Initialize(out PlayerCharacter _player)
        {
            _player = WorldComponent.Instance?.GetPlayerCharacter() ?? InstantiatePlayer();
            if (_player == null)
            {
                Debug.LogError("Player spawn failed!", this);
                return;
            }

            _player.GetPlayerHealth().OnDeath += RespawnPlayer;
            SpawnImmediate(_player);
        }

        private void RespawnPlayer()
        {
            StartCoroutine(SpawnDelayed());
        }

        private IEnumerator SpawnDelayed()
        {
            yield return new WaitForSeconds(m_spawnBalancing.RespawnDelay);

            RespawnImmediate(WorldComponent.Instance.GetPlayerCharacter());
        }
        
        private void SpawnImmediate(PlayerCharacter _player)
        {
            _player.transform.position = transform.position;
        }

        private void RespawnImmediate(PlayerCharacter _player)
        {
            SpawnImmediate(_player);
            _player.Revive();
        }

        private PlayerCharacter InstantiatePlayer()
        {
            return Instantiate(m_playerPrefab, transform.position, transform.rotation)?.GetComponent<PlayerCharacter>();
        }
    }
}