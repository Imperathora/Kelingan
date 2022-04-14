using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    public class MobileEnemySpawner : MonoBehaviour
    {
        [SerializeField] protected GameObject m_enemyPrefab = default;

        [SerializeField] private float m_respawnDelay = default;
        [SerializeField] private float m_resetDelay = default;

        [Tooltip("The maximum distance the enemy can walk from its spawn.")]
        [SerializeField] private float m_maxConfinementRadius = default;

        [Tooltip("The maximum distance the enemy will roam around its spawn.")]
        [SerializeField] private float m_maxRoamRange = default;

        private void Start() => WorldComponent.Instance.OnInitialization += InstantiateEnemy;

        private void InstantiateEnemy()
        {
            MobileEnemy enemy = Instantiate(m_enemyPrefab, transform)?.GetComponent<MobileEnemy>();
            if (enemy == null)
            {
                Debug.LogError("Enemy spawn failed!", this);
                return;
            }
            enemy.Initialize(transform.position, m_maxConfinementRadius, m_maxRoamRange);
            enemy.HealthComponent().OnDeath += () => { RespawnEnemy(enemy); };
            enemy.OnConfinementLeft += () => { ResetEnemy(enemy); };
        }

        private void ResetEnemy(MobileEnemy _enemy)
        {
            StartCoroutine(SpawnDelayed(_enemy, m_resetDelay));
        }

        private void RespawnEnemy(EnemyCharacter _enemy)
        {
            StartCoroutine(SpawnDelayed(_enemy, m_respawnDelay));
        }

        private IEnumerator SpawnDelayed(EnemyCharacter _enemy, float _delay)
        {
            yield return new WaitForSeconds(_delay);

            SpawnImmediate(_enemy);
        }

        public void SpawnImmediate(EnemyCharacter _enemy)
        {
            _enemy.transform.position = transform.position;
            _enemy.Revive();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, m_maxConfinementRadius);
            Gizmos.DrawWireSphere(transform.position, m_maxRoamRange);
        }
#endif
    }
}