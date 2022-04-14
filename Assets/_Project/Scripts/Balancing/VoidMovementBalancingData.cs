using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace OOB.Player
{
    public enum MoveMode { Instant, Acceleration }
    public enum TurnMode { Instant, Interpolation }
    public enum DashMode { Instant, Acceleration }

    [CreateAssetMenu(fileName = "Void Movement Balancing", menuName = "Balancing/Void Movement")]
    public class VoidMovementBalancingData : ScriptableObject
    {
        // ===== Movement =====

        [Header("Movement")]
        [SerializeField] private MoveMode m_movementMode = default;

        [HideMultiProperty("m_movementMode", "Instant", true)]
        [SerializeField] private float m_fixedMoveSpeed = default;

        [HideMultiProperty("m_movementMode", "Acceleration", true)]
        [SerializeField] private float m_moveAcceleration = default;
        [HideMultiProperty("m_movementMode", "Acceleration", true)]
        [SerializeField] private float m_maxMoveSpeed = default;


        // ===== Vertical Movement =====

        [Header("Vertical Movement")]
        [SerializeField] private MoveMode m_verticalMode = default;
        [HideMultiProperty("m_verticalMode", "Instant", true)]
        [SerializeField] private float m_fixedUpSpeed = default;
        [HideMultiProperty("m_verticalMode", "Instant", true)]
        [SerializeField] private float m_fixedDownSpeed = default;

        // ===== Sprint Movement =====

        [Header("Sprint Movement")]
        [SerializeField] private float m_sprintAcceleration = default;

        // ===== Turning =====

        [Header("Turning")]
        [Tooltip("Switch between instantaneous and visible rotations.")]
        [SerializeField] private TurnMode m_turnMode = default;

        [Tooltip("Is only applied during interpolation.")]
        [HideMultiProperty("m_turnMode", "Interpolation", true)]
        [SerializeField] private float m_turnSpeed = default;

        // ===== VoidIntro =====
        [Header("Void Intro")]
        [Tooltip("Void Intro Behavoir.")]
        [SerializeField] private float m_flyInTimer = default;
        [SerializeField] private float m_flyInDistance = default;

        // ===== Properties =====

        public MoveMode MoveMode => m_movementMode;
        public float MoveSpeed => m_movementMode == MoveMode.Instant ? m_fixedMoveSpeed : m_moveAcceleration;
        public float MaxMoveSpeed => m_maxMoveSpeed;

        public MoveMode VerticalMode => m_verticalMode;
        public float UpSpeed => m_fixedUpSpeed;
        public float DownSpeed => m_fixedDownSpeed;
        public float SprintAcceleration => m_sprintAcceleration;
        public TurnMode TurnMode => m_turnMode;
        public float TurnSpeed => m_turnSpeed;

        public float FlyInTimer => m_flyInTimer;
        public float FlyInDistance => m_flyInDistance;

    }
}