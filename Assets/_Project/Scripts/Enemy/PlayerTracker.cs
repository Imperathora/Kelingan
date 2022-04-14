using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy
{
    public class PlayerTracker : MonoBehaviour
    {
        [SerializeField] private EnemyAttackBalancingData m_attackBalancing = default;

        private float EnterAggressionRadius => m_attackBalancing.EnterAggressionRadius;
        private float ExitAggressionRadius => m_attackBalancing.ExitAggresionRadius;

        private Transform m_playerTransform;

        public event System.Action OnPlayerEnter;
        public event System.Action OnPlayerExit;

        public bool IsPlayerInRange { get; private set; }

        private void Start() => TryGetPlayer();
        private void TryGetPlayer()
        {
            if ((m_playerTransform = WorldComponent.Instance?.GetPlayerCharacter()?.transform) == null)
            {
                WorldComponent.Instance.OnInitialization += TryGetPlayer;
            }
        }

        private void FixedUpdate() => CheckPlayerDistance();
        private void CheckPlayerDistance()
        {
            if (Vector3.Distance(transform.position, m_playerTransform.position) < EnterAggressionRadius)
            {
                HandlePlayerInside();
            }
            else if (Vector3.Distance(transform.position, m_playerTransform.position) >= ExitAggressionRadius)
            {
                HandlePlayerOutside();
            }
        }

        private void HandlePlayerInside()
        {
            if (IsPlayerInRange)
                return;

            IsPlayerInRange = true;
            OnPlayerEnter?.Invoke();
        }

        private void HandlePlayerOutside()
        {
            if (!IsPlayerInRange)
                return;

            IsPlayerInRange = false;
            OnPlayerExit?.Invoke();
        }

        private void CleanUp()
        {
            OnPlayerEnter = null;
            OnPlayerExit = null;
        }

        private void OnDisable() => HandlePlayerOutside();
        private void OnDestroy() => CleanUp();

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnterAggressionRadius);
            Gizmos.DrawWireSphere(transform.position, ExitAggressionRadius);
        }
#endif
    }
}