using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB;
using OOB.Player;
using OOB.Sectors;
using UnityEditor;
using System;


public class PlayerAnimationController : MonoBehaviour
{

    private CharacterWorldMovement m_characterMovement;
    private VoidMovement m_voidMovement;
    private Animator m_playerAnimator;
    private PlayerCharacter m_playerCharacter;
    private SectorTracker m_sectorTracker;
    private PlayerHealthComponent m_playerHealth;
    


    public event Action OnPushingStarted;
    public event Action OnCelebrationEnded;
    public event Action OnSwitchActivationEnded;
    public event Action OnBeginSpinEnded;
    public event Action OnFadeOutStarted;


    public bool IsDeathAnimationPlaying()
    {
        if (m_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dieing"))
            return true;

        return false;
    }


    private void Start()
    {
        
        m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();
        m_playerAnimator = GetComponentInChildren<Animator>();
        m_characterMovement = m_playerCharacter.GetCharacterWorldMovement();
        m_voidMovement = m_playerCharacter.GetVoidMovement();
        m_sectorTracker = m_playerCharacter.GetSectorTracker();
        m_playerHealth = m_playerCharacter.GetPlayerHealth();
        m_sectorTracker.OnWorldExit += VoidAnimationStarted;
        m_sectorTracker.OnWorldExit += FallingAnimationStoped;
        m_sectorTracker.OnWorldEnter += VoidAnimationStoped;
        m_characterMovement.OnDashStarted += DashAnimation;
        m_characterMovement.OnCanFirstJump += FirstJump;
        m_characterMovement.OnCanSecondJump += SecondJump;
        m_characterMovement.OnTeeterStarted += TeeterStarted;
        m_characterMovement.OnTeeterStoped += TeeterStoped;
        m_characterMovement.OnIsFallingStarted += FallingAnimationStarted;
        m_characterMovement.OnIsFallingStoped += FallingAnimationStoped;
        m_characterMovement.OnStartSprinting += StartSprint;
        m_characterMovement.OnStopSprinting += StopSprint;
        m_characterMovement.OnStartEgg += StartEasterEgg;
        m_characterMovement.OnStopEgg += StopEasterEgg;
        m_characterMovement.OnStartCelebrating += CelebrationAnimation;
        m_playerCharacter.OnPushingObjectStart += StartPushing;
        m_playerCharacter.OnPushinObjectStop += StopPushing;
        m_playerCharacter.OnPlayerSpawn += BeginSpinAnimation;
        m_playerCharacter.OnStartGame += BeginSpinAnimation;
        m_voidMovement.OnVoidSprintStarted += VoidSprintAnimationStarted;
        m_voidMovement.OnVoidSprintStoped += VoidSprintAnimationStoped;
        m_playerHealth.OnDeath += DieAnimation;
        OnPushingStarted += StartPushing;
        

    }

    private void OnDestroy()
    {
        m_sectorTracker.OnWorldExit -= VoidAnimationStarted;
        m_sectorTracker.OnWorldExit -= FallingAnimationStoped;
        m_sectorTracker.OnWorldEnter -= VoidAnimationStoped;
        m_characterMovement.OnDashStarted -= DashAnimation;
        m_characterMovement.OnCanFirstJump -= FirstJump;
        m_characterMovement.OnCanSecondJump -= SecondJump;
        m_characterMovement.OnTeeterStarted -= TeeterStarted;
        m_characterMovement.OnTeeterStoped -= TeeterStoped;
        m_characterMovement.OnIsFallingStarted -= FallingAnimationStarted;
        m_characterMovement.OnIsFallingStoped -= FallingAnimationStoped;
        m_characterMovement.OnStartSprinting -= StartSprint;
        m_characterMovement.OnStopSprinting -= StopSprint;
        m_characterMovement.OnStartEgg -= StartEasterEgg;
        m_characterMovement.OnStopEgg -= StopEasterEgg;
        m_characterMovement.OnStartCelebrating -= CelebrationAnimation;
        m_playerCharacter.OnPushingObjectStart -= StartPushing;
        m_playerCharacter.OnPushinObjectStop -= StopPushing;
        m_playerCharacter.OnPlayerSpawn -= BeginSpinAnimation;
        m_playerCharacter.OnStartGame -= BeginSpinAnimation;
        m_voidMovement.OnVoidSprintStarted -= VoidSprintAnimationStarted;
        m_voidMovement.OnVoidSprintStoped -= VoidSprintAnimationStoped;
        m_playerHealth.OnDeath -= DieAnimation;
        OnPushingStarted -= StartPushing;
        OnCelebrationEnded = null;
    }

    void Update()
    {

        if (m_playerAnimator == null)
            return;

        if (!m_playerCharacter.IsInVoid() && m_characterMovement.IsPlayerGrounded())
        {
            MoveAnimation(m_characterMovement.PlayerVelocity());
        }

        if (m_characterMovement.m_moveInput.magnitude < 0.1f)
        {
            StopSprint();
        }
    }

    private void FallingAnimationStarted() => m_playerAnimator.SetBool("IsFalling", true);
    private void FallingAnimationStoped() => m_playerAnimator.SetBool("IsFalling", false);
    private void VoidAnimationStarted() => m_playerAnimator.SetBool("IsInVoid", true);
    private void VoidAnimationStoped() => m_playerAnimator.SetBool("IsInVoid", false);
    private void TeeterStarted() => m_playerAnimator.SetBool("IsTeeter", true);
    private void TeeterStoped() => m_playerAnimator.SetBool("IsTeeter", false);
    private void MoveAnimation(float _velocity) => m_playerAnimator.SetFloat("PlayerVelocity", _velocity);
    private void DashAnimation() => m_playerAnimator.Play("Base Layer.Dash", 0, 0.25f);
    private void StartPushing() => m_playerAnimator.SetBool("IsPushing", true);
    private void StopPushing() => m_playerAnimator.SetBool("IsPushing", false);
    private void VoidSprintAnimationStarted() => m_playerAnimator.SetBool("IsVoidSprinting", true);
    private void VoidSprintAnimationStoped() => m_playerAnimator.SetBool("IsVoidSprinting", false);
    private void DieAnimation() => m_playerAnimator.SetTrigger("IsDieing");
    private void CelebrationAnimation() => m_playerAnimator.SetTrigger("IsCelebrating");
    public void SwitchAnimation() => m_playerAnimator.SetTrigger("IsSwitchActivating");
    private void CelebrationEnded() => OnCelebrationEnded?.Invoke();
    private void SwitchAnimationEnded() => OnSwitchActivationEnded?.Invoke();
    private void StartEasterEgg() => m_playerAnimator.SetBool("IsEgg", true);
    private void StopEasterEgg() => m_playerAnimator.SetBool("IsEgg", false);
    private void BeginSpinAnimation() => m_playerAnimator.SetTrigger("IsBeginning");
    private void SpindAnimationEnded() => OnBeginSpinEnded?.Invoke();
    private void FadeOut() => OnFadeOutStarted?.Invoke();
    private void FirstJump()
    {
        m_playerAnimator.SetBool("IsFalling", false);
        m_playerAnimator.Play("Base Layer.Jump", 0, 0.25f);
    }

    private void SecondJump() => m_playerAnimator.Play("Base Layer.Double Jump", 0, 0.25f);

    private void StartSprint()
    {
        if(!m_playerCharacter.IsInVoid())
            m_playerAnimator.SetBool("IsSprinting", true);
    }
    private void StopSprint() 
    {
        if(!m_playerCharacter.IsInVoid())
            m_playerAnimator.SetBool("IsSprinting", false);
    }



}
