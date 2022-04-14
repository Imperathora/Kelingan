using UnityEngine;

[CreateAssetMenu(fileName = "Player Health Balancing Data", menuName = "Balancing/Player Health Balancing Data")]
public class PlayerHealthBalancingData : ScriptableObject
{
    [SerializeField] private int m_maxHealth = default;
    [SerializeField] private float m_immunityDuration = default;

    public int MaxHealth => m_maxHealth;
    public float ImmunityDuration => m_immunityDuration;
}
