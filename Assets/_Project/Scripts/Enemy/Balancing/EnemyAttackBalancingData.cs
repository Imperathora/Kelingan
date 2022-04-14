using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack Balancing Data", menuName = "Balancing/Enemy Attack Balancing Data")]
public class EnemyAttackBalancingData : ScriptableObject
{
    [Header("Damage")]
    [SerializeField] private int m_damageAmount = default;

    [Header("Attack Timing")]
    [SerializeField] private float m_startDelay = default;
    [SerializeField] private float m_coolDown = default;
    
    [Header("Aggro Range")]
    [SerializeField] private float m_enterAggressionRadius = default;
    [SerializeField] private float m_exitAggressionRadius = default;

    public int DamageAmount { get { return m_damageAmount; } }

    public float StartDelay { get { return m_startDelay; } }
    public float Cooldown { get { return m_coolDown; } }

    public float EnterAggressionRadius => m_enterAggressionRadius;
    public float ExitAggresionRadius => m_exitAggressionRadius;
}
