using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB;
using OOB.Player;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_doubleJumpDust;
    [SerializeField] private ParticleSystem m_jumpDustOne;
    [SerializeField] private ParticleSystem m_jumpDustTwo;
    [SerializeField] private ParticleSystem m_runningDust;
    [SerializeField] private ParticleSystem m_sprintingDust;
    [SerializeField] private ParticleSystem m_dashDust;

    private PlayerCharacter m_playerCharacter;
    private CharacterWorldMovement m_characterMovement;

    private void Start()
    {
        m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();
        m_characterMovement = m_playerCharacter.GetCharacterWorldMovement();
        m_characterMovement.OnCanSecondJump += DoubleJumpDustParticle;
        m_characterMovement.OnCanFirstJump += JumpDustParticle;
        m_characterMovement.OnStartSprintingEffect += StartSprintDustParticle;
        m_characterMovement.OnStopSprintingEffect += StopSprintDustParticle;
        m_characterMovement.OnStartRunning += PlayRunningDustParticle;
        m_characterMovement.OnStopRunning += StopRunningDustParticle;
        m_characterMovement.OnDashStarted += StartDashParticle;

    }

    private void OnDestroy()
    {
        m_characterMovement.OnCanSecondJump -= DoubleJumpDustParticle;
        m_characterMovement.OnCanFirstJump -= JumpDustParticle;
        m_characterMovement.OnStartSprintingEffect -= StartSprintDustParticle;
        m_characterMovement.OnStopSprintingEffect -= StopSprintDustParticle;
        m_characterMovement.OnStartRunning -= PlayRunningDustParticle;
        m_characterMovement.OnStopRunning -= StopRunningDustParticle;
        m_characterMovement.OnDashStarted -= StartDashParticle;

    }

    private void StartDashParticle()
    {
        PlayParticleEffect(m_dashDust);
    }

    private void PlayRunningDustParticle()
    {

        PlayParticleEffect(m_runningDust);
    }

    private void StopRunningDustParticle()
    {
        StopParticleEffect(m_runningDust);
    }
    private void DoubleJumpDustParticle()
    {
        PlayParticleEffect(m_doubleJumpDust);
    }

    private void JumpDustParticle()
    {
        PlayParticleEffect(m_jumpDustOne);
        PlayParticleEffect(m_jumpDustTwo);
    }

    private void StartSprintDustParticle()
    {
        PlayParticleEffect(m_sprintingDust);
    }

    private void StopSprintDustParticle()
    {
        StopParticleEffect(m_sprintingDust);
    }


    private void PlayParticleEffect(ParticleSystem _particle)
    {
        _particle.GetComponent<ParticleSystem>().Play();
    }

    private void StopParticleEffect(ParticleSystem _particle)
    {
        _particle.GetComponent<ParticleSystem>().Stop();
    }
}
