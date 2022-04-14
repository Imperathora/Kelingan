using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Character;

namespace OOB.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] protected GameObject m_enemyPrefab = default;

        [SerializeField] private float m_respawnDelay = default;

        private void Start() => WorldComponent.Instance.OnInitialization += InstantiateEnemy;

        private void InstantiateEnemy()
        {
            StaticEnemy enemy = Instantiate(m_enemyPrefab, transform)?.GetComponent<StaticEnemy>();
            if (enemy == null)
            {
                Debug.LogError("Enemy spawn failed!", this);
                return;
            }
            enemy.Initialize();
            enemy.HealthComponent().OnDeath += () => RespawnEnemy(enemy);
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
        }
#endif
    }
}