using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System.Data.Common;
using TMPro;
using System.Drawing;
using OOB.Sectors;
using OOB.Collectible;

namespace OOB.Player
{
    public class CharacterWorldMovement : PlayerMovementBase
    {
        [SerializeField] private Framework.Data.Vector3Container m_cameraDirection = default;
        [SerializeField] private WorldMovementBalancingData m_movementData = default;
        [SerializeField] private SectorTracker m_sectorTracker = default;

        private CapsuleCollider m_collider;
        private PlayerInputHandler m_inputHandler;
        private CollectibleCounter m_collectibleCounter;

        [HideProperty] public Vector2 m_moveInput;

        public event Action OnDashStarted;
        public event Action OnDashFinished;
        public event Action OnCanFirstJump;
        public event Action OnCanSecondJump;
        public event Action OnTeeterStarted;
        public event Action OnTeeterStoped;
        public event Action OnIsFallingStarted;
        public event Action OnIsFallingStoped;
        public event Action OnStartSprinting;
        public event Action OnStartSprintingEffect;
        public event Action OnStopSprintingEffect;
        public event Action OnStopSprinting;
        public event Action OnStartRunning;
        public event Action OnStopRunning;
        public event Action OnStartCelebrating;
        public event Action OnStopCelebrating;

        public event Action OnStartEgg;
        public event Action OnStopEgg;
        private float m_eggTimer;

        private bool m_isGrounded = default;
        private float m_jumpTimeCounter;
        private bool m_isJumpPressed;
        private bool m_isJumped;
        private bool m_isSprinting;
        private bool m_canSprint = true;
        private bool m_canDash = true;
        private bool m_canJump = true;
        private bool m_isDashing;
        private bool m_isDashingInAerial;
        private bool m_isDashOnCooldown;
        private float m_timer;
        private float m_delayJumpTimer;
        private float m_playerVelocity;
        private bool m_hasUsedSecondJump;
        private bool m_isTwodMovement;
        private bool m_isCelebrating;

        private Vector3 playerFirstJumpDirection = default;

        private void StartJump() => m_isJumpPressed = true;
        private void EndJump() => m_isJumpPressed = false;
        public float PlayerVelocity() => m_playerVelocity;
        private void TwodMovementStarting() => m_isTwodMovement = true;
        private void TwodMovementStoped() => m_isTwodMovement = false;
        public bool DashOnCooldown() => m_isDashOnCooldown;
        public bool IsPlayerGrounded() => m_isGrounded;
        public bool IsPlayerJumping() => m_isJumpPressed;
        public void StartCelebration() => m_isCelebrating = true;
        public PlayerInputHandler GetInputHandler() => m_inputHandler;
        public void CanSprint(bool _cansprint)
        {
            m_canSprint = _cansprint;
        }

        public void CanDash(bool _candash)
        {
            m_canDash = _candash;
        }
        public void Initialize(Rigidbody _rigidbody, CapsuleCollider _collider, PlayerInputHandler _inputHandler, PlayerAttack _attack, CameraSwitch _cameraSwitch, PlayerAnimationController _animationController)
        {
            m_rigidbody = _rigidbody;
            m_collider = _collider;
            m_inputHandler = _inputHandler;
            _cameraSwitch.TwodMovementStarting += TwodMovementStarting;
            _cameraSwitch.TowdMovementStoped += TwodMovementStoped;
            _attack.OnEnemyHit += HandleAttackLanded;
            _animationController.OnCelebrationEnded += CelebrationEnded;
        }

        private void OnEnable()
        {
            m_collectibleCounter = GameManager.Instance?.GetCollectibleCounter();

            if(m_collectibleCounter == null)
                return;

            m_collectibleCounter.OnStarCollected += StartCelebration;
            m_inputHandler.OnJumpPressed.Event += TryJump;
            m_inputHandler.OnDashPressed.Event += StartDash;
            m_inputHandler.OnJumpReleased.Event += EndJump;
            m_inputHandler.OnJumpPressed.Event += StartJump;
            m_inputHandler.OnSprintPressed.Event += StartSprint;
            m_inputHandler.OnSprintReleased.Event += StopSprint;
            m_sectorTracker.OnWorldEnter += AlignPlayer;
            m_isJumpPressed = false;
            m_isJumped = false;
        }

        private void OnDisable()
        {
            if (m_collectibleCounter == null)
                return;

            m_rigidbody.velocity = Vector3.zero;
            m_playerVelocity = 0f;
            OnStopSprinting?.Invoke();
            m_inputHandler.OnJumpPressed.Event -= TryJump;
            m_inputHandler.OnDashPressed.Event -= StartDash;
            m_inputHandler.OnJumpReleased.Event -= EndJump;
            m_inputHandler.OnJumpPressed.Event -= StartJump;
            m_collectibleCounter.OnStarCollected -= StartCelebration;
            m_isDashing = false;
            m_isDashOnCooldown = false;
        }

        private void FixedUpdate()
        {
            
            m_isGrounded = IsGrounded();

            if (m_isGrounded)
            {
                OnIsFallingStoped?.Invoke();
                m_isJumped = false;
                m_isDashingInAerial = false;
                m_delayJumpTimer = 0f;
                OnStartSprintingEffect?.Invoke();

                if (m_isCelebrating)
                {
     
                    m_inputHandler.DisableInput(true, false);
                    StartCoroutine(WaitForCamera(1f, OnStartCelebrating,true));
                    m_isCelebrating = false;
                }
            }
            else
            {
                if (!IsPlayerGroundedNotFalling())
                    OnIsFallingStarted?.Invoke();

                OnTeeterStoped?.Invoke();
                m_delayJumpTimer += Time.fixedDeltaTime;
                OnStopSprintingEffect?.Invoke();
            }

            if (m_isDashOnCooldown)
            {
                m_timer += Time.fixedDeltaTime;
                if (m_timer >= m_movementData.DashCooldown)
                    m_isDashOnCooldown = false;
            }

            if (m_isDashing)
            {
                if (IsSlopeToSteep(0.9f))
                    m_rigidbody.velocity += Vector3.down * (m_movementData.Gravity * m_movementData.DashGravity) * Time.fixedDeltaTime;

                m_timer += Time.fixedDeltaTime;
                if (m_timer >= m_movementData.DashTime)
                    StopDash();
            }
            else
            {
                TurnAndMove();
                Fall();

            }

            if (!m_isSprinting || !m_canSprint)
            {
                StopSprint();
            }

            if (IsSlopeToSteep(m_movementData.SteepDegree))
            {
                m_canJump = false;

            }
            else
            {
                m_canJump = true;

            }


            if (IsSlopeToSteep(m_movementData.SteepDegree) && !m_isJumped && !m_isJumpPressed)
            {
                m_rigidbody.velocity += Vector3.down * m_movementData.SlideForce * Time.fixedDeltaTime;
                OnIsFallingStoped?.Invoke();
            }

            if (m_moveInput.x == 0 && m_moveInput.y == 0)
            {
                m_eggTimer += Time.deltaTime;

                if (m_eggTimer > 120f)
                    OnStartEgg?.Invoke();
            } else
            {
                OnStopEgg?.Invoke();
                m_eggTimer = 0f;
            }
        }
        private void AlignPlayer()
        {
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, eulerRotation.y, eulerRotation.z);
        }

        private void Fall()
        {
            Vector3 yVelocity = Vector3.zero;

            if (!m_isGrounded)
            {

                if (m_isJumpPressed && m_jumpTimeCounter >= 0)
                {
                    m_isJumped = true;
                    m_jumpTimeCounter -= Time.fixedDeltaTime;
                }
                else
                {
                    m_rigidbody.AddForce(Vector3.down * m_movementData.Gravity * m_rigidbody.mass , m_movementData.ForceMode);

                    yVelocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_movementData.Gravity);
                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, yVelocity.y, m_rigidbody.velocity.z);

                    //if (m_rigidbody.velocity.y > m_movementData.MaxJumpVelocity)
                    //{
                    //    //m_rigidbody.velocity = m_rigidbody.velocity.With(y: m_movementData.MaxJumpVelocity);
                    //}
                    //else if (m_rigidbody.velocity.y < -m_movementData.GravityMaxVelocity)
                    //{
                    //    m_rigidbody.velocity = m_rigidbody.velocity.With(y: -m_movementData.GravityMaxVelocity);
                    //}
                }
            }
        }

        private bool IsSlopeToSteep(float _scalar)
        {
            bool toSteep = false;
            RaycastHit hit;

            toSteep |= Physics.Raycast(m_collider.bounds.center + Vector3.down * m_collider.height / 2, transform.forward, out hit, 1.0f);
            toSteep |= Physics.Raycast(m_collider.bounds.center + Vector3.down * m_collider.height / 2, transform.forward + transform.right, out hit, 1.0f);
            toSteep |= Physics.Raycast(m_collider.bounds.center + Vector3.down * m_collider.height / 2, transform.forward - transform.right, out hit, 1.0f);

            if (toSteep)
            {
                if (Vector3.Dot(Vector3.up, hit.normal) < _scalar && Vector3.Dot(Vector3.up, hit.normal) > 0.4f)
                {
                    return true;
                }

            }
            return false;
        }

        public Tuple<int, Collider> GetLayerMaskAndCollider()
        {
            RaycastHit hit;
            bool hashit = Physics.Raycast(m_collider.bounds.center, Vector3.down, out hit, 5f);

            if (!hashit)
                return null;

            return Tuple.Create(hit.transform.gameObject.layer, hit.collider);
        }


        private bool HasRaycastHitGround(Vector3 _origin, out RaycastHit _hit, ref float shortestDistance)
        {
            bool hasHit = Physics.Raycast(_origin, Vector3.down, out _hit, m_collider.height / 2 + 0.2f);
            if (_hit.distance > 0f && _hit.distance < shortestDistance)
                shortestDistance = _hit.distance;


            return hasHit;
        }

        private bool IsGrounded()
        {
            bool hasFoundGround = false;

            float shortestDistance = float.MaxValue;

            RaycastHit hit = new RaycastHit();
            hasFoundGround |= HasRaycastHitGround(m_collider.bounds.center, out hit, ref shortestDistance);

            hasFoundGround |= HasRaycastHitGround(m_collider.bounds.center + Vector3.forward * m_collider.radius / 2, out hit, ref shortestDistance);

            hasFoundGround |= HasRaycastHitGround(m_collider.bounds.center + Vector3.back * m_collider.radius / 2, out hit, ref shortestDistance);

            hasFoundGround |= HasRaycastHitGround(m_collider.bounds.center + Vector3.left * m_collider.radius / 2, out hit, ref shortestDistance);

            hasFoundGround |= HasRaycastHitGround(m_collider.bounds.center + Vector3.right * m_collider.radius / 2, out hit, ref shortestDistance);

            
            return hasFoundGround;
        }

        private bool IsFrontPlayerGrounded()
        {
            

            RaycastHit hit = new RaycastHit();
            bool hasHit = Physics.Raycast(m_collider.bounds.center + transform.forward * m_collider.radius, Vector3.down, out hit, m_collider.height / 2 + 3f);

            
            return hasHit;
        }

        private bool IsPlayerGroundedNotFalling()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(m_collider.bounds.center, Vector3.down, out hit, m_collider.height);
            return hasHit;
        }

        private void TurnAndMove()
        {
            Vector3 moveDirection = transform.forward;
            Vector3 velocity = m_rigidbody.velocity;

            if (m_moveInput.x != 0 || m_moveInput.y != 0)
            {
                if (!m_isSprinting && m_isGrounded)
                {
                    OnStartRunning?.Invoke();
                } else
                {
                    OnStopRunning?.Invoke();
                }

                if (m_moveInput.magnitude > 1)
                    m_moveInput.Normalize();

                if (!m_isTwodMovement)
                {
                    Vector3 turnDirection = CalcLookDirectionFlat(m_moveInput, m_cameraDirection.Value);
                    TurnTowards(transform.forward, turnDirection, m_movementData.TurnMode, m_movementData.TurnSpeed);
                    moveDirection = turnDirection;
                } else
                {
                    Vector3 TwodMovement = default;
                    TwodMovement.x = m_moveInput.y;
                    TwodMovement.z = -m_moveInput.x;

                    TurnTowards(transform.forward, TwodMovement, m_movementData.TurnMode, m_movementData.TurnSpeed);
                    moveDirection = TwodMovement;
                }
                

            } 


            Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);


            if (m_isGrounded)
            {
                ApplyMoveDirectionToVelocity(ref horizontalVelocity, m_moveInput, moveDirection, m_movementData.GroundAcceleration);



                horizontalVelocity = horizontalVelocity * (1 - Time.fixedDeltaTime * m_movementData.Drag);

                horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, m_movementData.GroundMaxSpeed);

                if (m_isSprinting && m_canSprint)
                    ApplyMoveDirectionToVelocity(ref horizontalVelocity, m_moveInput, moveDirection, m_movementData.SprintAcceleration);

                if (m_moveInput.x != 0 || m_moveInput.y != 0)
                {
                    
                    if (m_moveInput.magnitude <= m_movementData.MinMagnitude && !IsFrontPlayerGrounded())
                    {
                        horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, m_movementData.SlowTime);
                        OnTeeterStarted?.Invoke();
                    }

                    if (IsFrontPlayerGrounded())
                       OnTeeterStoped?.Invoke();
                }

            }
            else
            {
                ApplyMoveDirectionToVelocity(ref horizontalVelocity, m_moveInput, moveDirection, m_movementData.AerialAcceleration);

                horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, m_movementData.AerialMaxSpeed);

            }

            if (m_moveInput.x == 0 && m_moveInput.y == 0)
            {
                OnStopRunning?.Invoke();
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, m_movementData.AerialDeacceleration);

            }


            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.y;

      

            m_playerVelocity = horizontalVelocity.magnitude / m_movementData.GroundMaxSpeed;
            m_rigidbody.velocity = velocity;


            if (m_moveInput.x == 0 && m_moveInput.y == 0 && m_isGrounded)
            {
                if (m_rigidbody.velocity.magnitude <= m_movementData.MinSpeed)
                {
                    m_rigidbody.velocity = Vector3.zero;
                }
            }

        }


        private void ApplyMoveDirectionToVelocity(ref Vector2 _velocity, Vector2 _inputAxis, Vector3 _moveDirection, float _acceleration)
        {

            _velocity.x += _moveDirection.x * _inputAxis.magnitude * Time.fixedDeltaTime * _acceleration;
            _velocity.y += _moveDirection.z * _inputAxis.magnitude * Time.fixedDeltaTime * _acceleration;
        }

        private void StartDash()
        {
            if (m_isDashing || m_isDashOnCooldown || m_isDashingInAerial || !m_canDash)
                return;

       
            m_rigidbody.velocity = transform.forward * m_movementData.DashDistance / m_movementData.DashTime;
            m_rigidbody.velocity *= (1 - Time.fixedDeltaTime * m_movementData.Drag);
            m_isDashing = true;
            m_timer = 0f;
            OnDashStarted?.Invoke();

            if (!m_isGrounded)
                m_isDashingInAerial = true;
        }

        private void HandleAttackLanded()
        {
            m_isDashing = false;
            m_timer = 0f;
            m_hasUsedSecondJump = false;
            OnDashFinished?.Invoke();
        }

        private void StopDash()
        {
            m_isDashing = false;
            m_isDashOnCooldown = true;
            m_timer = 0f;
            OnDashFinished?.Invoke();
        }
        private void Jump(float _jumpForce) => m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, Mathf.Sqrt(2 * _jumpForce), m_rigidbody.velocity.z);
        private bool CanFirstJump()
        {
            if (m_movementData.DelayJumpTimer > 0)
               return m_delayJumpTimer <= m_movementData.DelayJumpTimer && !m_isJumped;

            return !m_isJumped;
        }

        private bool CanSecondJump => !m_hasUsedSecondJump;
        private void TryJump()
        {
            if (!m_canJump)
                return;

            if (CanFirstJump())
            {
                Jump(m_movementData.JumpForce);
                m_hasUsedSecondJump = false;
                m_jumpTimeCounter = m_movementData.JumpTime;
                playerFirstJumpDirection = transform.forward;
                OnCanFirstJump?.Invoke();
            } else if (CanSecondJump)
            {
                float jumpAngle = Vector3.Angle(transform.forward, playerFirstJumpDirection);

                if (jumpAngle >= m_movementData.SecondJumpInterruptionAngle)
                    m_rigidbody.velocity = Vector3.zero;

                Jump(m_movementData.SecondJumpForce);
                m_jumpTimeCounter = m_movementData.JumpTime;
                m_hasUsedSecondJump = true;
                OnCanSecondJump?.Invoke();
            }

        }

        private void StartSprint()
        {
            if (m_moveInput.x != 0 || m_moveInput.y != 0)
            {
                m_isSprinting = true;
                OnStartSprinting?.Invoke();
                OnStartSprintingEffect?.Invoke();
            }
        }
        private void StopSprint()
        {
            m_isSprinting = false;
            OnStopSprinting?.Invoke();
            OnStopSprintingEffect?.Invoke();
        }
        private void OnDestroy()
        {
            OnDashStarted = null;
            OnDashFinished = null;

            m_inputHandler.OnSprintPressed.Event -= StartSprint;
            m_inputHandler.OnSprintReleased.Event -= StopSprint;
            m_sectorTracker.OnWorldEnter -= AlignPlayer;
        }
        private void CelebrationEnded()
        {
            StartCoroutine(WaitForCamera(2f, OnStopCelebrating, false));
            
        }
        private IEnumerator WaitForCamera(float _time, Action _action, bool _fadeIn)
        {
            if (_fadeIn)
            {
                yield return new WaitForSeconds(_time);
                _action?.Invoke();
            } else
            {
                _action?.Invoke();
                yield return new WaitForSeconds(_time);
                m_inputHandler.DisableInput(false, false);
            }


        }

    }
}