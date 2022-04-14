using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB
{
    [CreateAssetMenu(fileName = "Movement Balancing Data", menuName = "Balancing/Movement Balancing Data")]
    public class MovementBalancingData : ScriptableObject
    {
        [SerializeField] private float m_groundAcceleration = default;
        [SerializeField] private float m_groundMaxSpeed = default;
        [SerializeField] private float m_aerialAcceleration = default;
        [SerializeField] private float m_aerialMaxSpeed = default;
        [SerializeField] private float m_voidAcceleration = default;
        [SerializeField] private float m_voidMaxSpeed = default;
        [SerializeField] private float m_jumpForce = default;
        [SerializeField] private float m_gravity = default;
        [SerializeField] private float m_dashDistance = default;
        [SerializeField] private float m_dashTime = default;

        public float GroundAcceleration { get { return m_groundAcceleration; } }
        public float GroundMaxSpeed { get { return m_groundMaxSpeed; } }
        public float AerialAcceleration { get { return m_aerialAcceleration; } }
        public float AerialMaxSpeed { get { return m_aerialMaxSpeed; } }
        public float VoidAcceleration { get { return m_voidAcceleration; } }
        public float VoidMaxSoeed { get { return m_voidMaxSpeed; } }
        public float JumpForce { get { return m_jumpForce; } }
        public float Gravity { get { return m_gravity; } }
        public float DashDistance { get { return m_dashDistance; } }
        public float DashTime { get { return m_dashTime; } }
    }
}