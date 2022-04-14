using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OOB.Player;
using Rewired;

namespace OOB.Collectible
{
    
    public enum Type
    {
        Coin,
        Star
    }

    public class CollectibleComponent : MonoBehaviour
    {
        [SerializeField] private Type m_type = default;
        [SerializeField] private UnityEvent OnCollection = default;
        [SerializeField] private WorldMovementBalancingData m_movementData = default;

        private PlayerCharacter m_playerCharacter;
        private bool m_isMagnitze;

        public Type GetCollectibleType() => m_type;

        private void Update()
        {
            if(m_isMagnitze)
                MoveToPlayer();
        }
        private void OnTriggerEnter(Collider other)
        {
            m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();

            if (other.transform.IsChildOf(m_playerCharacter.transform))
            {
                if(m_type == Type.Coin)
                {
                    m_isMagnitze = true;
                } else
                {
                    GetCollectable();
                }
                    
            }
        }

        private void MoveToPlayer()
        {

            Vector3 direction = (m_playerCharacter.transform.position - transform.parent.position);

            if (direction.magnitude < .5)
            {
                
                GetCollectable();
            }

            float speed = m_movementData.SprintAcceleration / 3;
            speed = speed * Time.deltaTime * .5f;
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, m_playerCharacter.transform.position, speed);
        }


        private void GetCollectable()
        {
            GameManager.Instance?.GetCollectibleCounter()?.Collect(m_type);
            OnCollection.Invoke();
        }
    }
}