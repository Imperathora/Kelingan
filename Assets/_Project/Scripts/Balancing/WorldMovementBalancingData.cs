using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace OOB.Player
{
    [CreateAssetMenu(fileName = "World Movement Balancing", menuName = "Balancing/World Movement")]
    public class WorldMovementBalancingData : ScriptableObject
    {

        // ===== Turning =====

        [Header("Turning")]
        [Tooltip("Switch between instantaneous and visible rotations.")]
        [SerializeField] private TurnMode m_turnMode = default;

        [Tooltip("Is only applied during interpolation.")]
        [HideMultiProperty("m_turnMode", "Interpolation", true)]
        [SerializeField] private float m_turnSpeed = default;


        // ===== Ground Movement =====

        [Header("Ground Movement")]
        [SerializeField] private float m_groundAcceleration = default;
        [SerializeField] private float m_groundMaxSpeed = default;

        // ===== Sprint Movement =====

        [Header("Sprint Movement")]
        [SerializeField] private float m_sprintAcceleration = default;

        // ===== Aerial Movement =====

        [Header("Aerial Movement")]
        [SerializeField] private float m_aerialAcceleration = default;
        [SerializeField] private float m_aerialMaxSpeed = default;

        // ===== Aerial Deaccelerate =====

        [Header("Aerial Deaccelerate")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_aerialDeaccelerate = default;

        // ===== Jump =====

        [Header("Jump")]
        [SerializeField] private float m_jumpForce = default;
        [SerializeField] private float m_maxJumpVelocity = default;

        // ===== Second Jump =====

        [Header("Second Jump")]
        [SerializeField] private float m_secondJumpForce = default;

        // ===== Second Jump Interruption Angle =====

        [Header(" Second Jump Interruption Angle")]
        [SerializeField] private float m_secondJumpInterruptionAngle = default;

        // ===== JumpTime =====

        [Header("Jump Time")]
        [Range(0.0f, 1f)]
        [SerializeField] private float m_jumpTime = default;

        // ===== Jump Force Mode =====

        [Header("Fall Force")]
        [Tooltip("Switch between Force Modes of falling.")]
        [SerializeField] private ForceMode m_forceMode = default;

        // ===== Delayed Jump Timer =====

        [Header("Delayed Jump Timer")]
        [SerializeField] private float m_delayedJumpTimer = default;

        // ===== Gravity =====

        [Header("Gravity")]
        [SerializeField] private float m_gravity = default;
        [SerializeField] private float m_gravityMaxVelocity = default;

        // ===== Drag =====

        [Header("Drag")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float m_drag = default;

        // ===== Min Speed =====

        [Header("Minimum Speed")]
        [SerializeField] private float m_minSpeed = default;

        // ===== Dash =====

        [Header("Dash")]
        [SerializeField] private float m_dashDistance = default;
        [SerializeField] private float m_dashTime = default;
        [SerializeField] private float m_dashCooldown = default;
        [SerializeField] private float m_dashGravity = default;

        // ===== Teeter =====

        [Header("Teeter")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_minStickIntensity = default;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_slowTime = default;

        // ===== Slope =====

        [Header("Slope")]
        [Tooltip("0.7 ~ 45 degree")]
        [SerializeField] private float m_steepDegree = default;
        [SerializeField] private float m_slideForce = default;
 

        // ===== Properties =====

        public TurnMode TurnMode => m_turnMode;
        public float TurnSpeed => m_turnSpeed;
        public float GroundAcceleration => m_groundAcceleration;
        public float GroundMaxSpeed => m_groundMaxSpeed;
        public float SprintAcceleration => m_sprintAcceleration;
        public float AerialAcceleration => m_aerialAcceleration;
        public float AerialDeacceleration => m_aerialDeaccelerate;
        public float AerialMaxSpeed => m_aerialMaxSpeed;
        public float JumpForce => m_jumpForce;
        public float SecondJumpForce => m_secondJumpForce;
        public float SecondJumpInterruptionAngle => m_secondJumpInterruptionAngle;
        public ForceMode ForceMode => m_forceMode;
        public float JumpTime => m_jumpTime;
        public float DelayJumpTimer => m_delayedJumpTimer;
        public float MaxJumpVelocity => m_maxJumpVelocity;
        public float Gravity => m_gravity;
        public float Drag => m_drag;
        public float MinSpeed => m_minSpeed;
        public float GravityMaxVelocity => m_gravityMaxVelocity;
        public float DashDistance => m_dashDistance;
        public float DashTime => m_dashTime;
        public float DashCooldown => m_dashCooldown;
        public float DashGravity => m_dashGravity;
        public float MinMagnitude => m_minStickIntensity;
        public float SlowTime => m_slowTime;
        public float SteepDegree => m_steepDegree;
        public float SlideForce => m_slideForce;


    }
}