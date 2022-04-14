using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Player;
using OOB;
using OOB.Sectors;
using OOB.Collectible;

public class PlayerSoundController : MonoBehaviour
{

    [SerializeField] private CharacterWorldMovement m_worldMovement = default;
    [SerializeField] private PlayerCollector m_playerCollector = default;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip[] m_snowSteps;
    [SerializeField] private AudioClip[] m_stoneSteps;
    [SerializeField] private AudioClip[] m_waterSteps;
    [SerializeField] private AudioClip[] m_templeSteps;
    [SerializeField] private AudioClip m_jumpSound;
    [SerializeField] private AudioClip m_dashSound;
    [SerializeField] private AudioClip m_voidButtonSound;
    [SerializeField] private AudioClip m_enterVoid;
    [SerializeField] private AudioClip m_coinSound;
    [SerializeField] private AudioClip m_movingCrateSound;
    [SerializeField] private AudioClip m_damageSound;
    [SerializeField] private AudioClip m_deathSound;
    [SerializeField] private AudioClip m_enterPortalSound;
    [SerializeField] private AudioClip m_starCollectedSound;
    [SerializeField] private AudioClip m_switchActivationSound;
    [SerializeField] private int m_snowLayer;
    [SerializeField] private int m_stoneLayer;
    [SerializeField] private int m_waterLayer;
    [SerializeField] private int m_templeLayer;
    [SerializeField] private TerrainDetector m_terrainDetector;

    private AudioClip clip;
    private PlayerCharacter m_playerCharacter;
    private PlayerHealthComponent m_playerHealth;
    private SectorTracker m_sectorTracker;
    private CollectibleCounter m_collectibleCounter;

    private void Start()
    {
        m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();
        m_collectibleCounter = GameManager.Instance?.GetCollectibleCounter();

        if (m_collectibleCounter == null)
            return;

        m_collectibleCounter.OnStarCollected += () => PlaySound(m_starCollectedSound);
        m_playerHealth = m_playerCharacter.GetPlayerHealth();
        m_sectorTracker = m_playerCharacter.GetSectorTracker();
        m_worldMovement.GetInputHandler().OnVoidPressed.Event += () => PlaySound(m_voidButtonSound);
        m_worldMovement.OnCanFirstJump += () => PlaySound(m_jumpSound);
        m_worldMovement.OnCanSecondJump += () => PlaySound(m_jumpSound);
        m_worldMovement.OnDashStarted += () => PlaySound(m_dashSound);
        m_playerCollector.OnCoinCollected += () => PlaySound(m_coinSound);
        m_playerCharacter.OnPushingObjectStart += () => StartPlaySoundLoop(m_movingCrateSound);
        m_playerCharacter.OnPushinObjectStop += StopPlaySoundLoop;
        m_sectorTracker.OnWorldExit += () => PlaySound(m_enterVoid);
        m_playerHealth.OnDamageTaken += () => PlaySound(m_damageSound);
        m_playerHealth.OnDeath += () => PlaySound(m_deathSound);

    }

    private void OnDestroy()
    {
        if (m_sectorTracker == null)
            return;

        m_worldMovement.OnCanFirstJump -= () => PlaySound(m_jumpSound);
        m_worldMovement.OnCanSecondJump -= () => PlaySound(m_jumpSound);
        m_worldMovement.OnDashStarted -= () => PlaySound(m_dashSound);
        m_playerCollector.OnCoinCollected -= () => PlaySound(m_coinSound);
        m_worldMovement.GetInputHandler().OnVoidPressed.Event -= () => PlaySound(m_voidButtonSound);
        m_sectorTracker.OnWorldExit -= () => PlaySound(m_enterVoid);
        m_playerCharacter.OnPushingObjectStart -= () => PlaySound(m_movingCrateSound);
        m_playerCharacter.OnPushinObjectStop += StopPlaySoundLoop;
        m_playerHealth.OnDamageTaken -= () => PlaySound(m_damageSound);
        m_playerHealth.OnDeath -= () => PlaySound(m_deathSound);
        m_collectibleCounter.OnStarCollected -= () => PlaySound(m_starCollectedSound);
    }
    public void Step(float _speed)
    {

        float? speed = _speed;
        int? layer = m_worldMovement.GetLayerMaskAndCollider()?.Item1;

        if (!m_worldMovement.IsPlayerGrounded() || speed == null || layer == null)
            return;

        if (CheckLayer(m_snowLayer) && m_terrainDetector.GetColliderLayer() == m_snowLayer)
        {
            clip = GetRandomClip(Sounds.stonefoot, 0);
        }
        else
        {
            clip = GetRandomClip(Sounds.emtpy, m_worldMovement.GetLayerMaskAndCollider().Item1);
        }

        if (CheckLayer(m_stoneLayer) && m_terrainDetector.GetColliderLayer() == m_stoneLayer)
        {
            clip = GetRandomClip(Sounds.snowfoot, 0);
        }


        if (CheckLayer(m_waterLayer))
            clip = GetRandomClip(Sounds.water, 0);

        if (CheckLayer(m_templeLayer))
            clip = GetRandomClip(Sounds.temple, 0);



        if (GetMovementState(m_worldMovement.PlayerVelocity()) == GetMovementState(_speed))
        {
            m_audioSource.PlayOneShot(clip);
        }

    }

    private Movement GetMovementState(float _speed)
    {
        if(_speed <= .5f)
            return Movement.walk;

        if(_speed <= 1)
            return Movement.run;
            
        if(_speed > 1)
            return Movement.sprint;

        return Movement.empty;
    }

    private AudioClip GetRandomClip(Sounds _sound, int _layer)
    {
        if (_sound == Sounds.snowfoot || _layer == m_snowLayer)
            return m_snowSteps[Random.Range(0, m_snowSteps.Length)];

        if (_sound == Sounds.stonefoot || _layer == m_stoneLayer)
            return m_stoneSteps[Random.Range(0, m_stoneSteps.Length)];

        if (_sound == Sounds.water || _layer == m_waterLayer)
            return m_waterSteps[Random.Range(0, m_waterSteps.Length)];

        if (_sound == Sounds.temple || _layer == m_templeLayer)
            return m_templeSteps[Random.Range(0, m_templeSteps.Length)];

        return null;
    }

    private bool CheckLayer(int _layer)
    {
        if (m_worldMovement.GetLayerMaskAndCollider()?.Item1 == 0)
            return false;

       return m_worldMovement.GetLayerMaskAndCollider()?.Item1 == _layer;
    }
    public void PlayPortalSound()
    {
        if (m_audioSource == null)
            return;

        PlaySound(m_enterPortalSound);
    }

    public void PlaySwitchActivationSound()
    {
        if (m_audioSource == null)
            return;

        PlaySound(m_switchActivationSound);
    }
    private void PlaySound(AudioClip _clip)
    {
        if (m_audioSource == null)
            return;

        m_audioSource.PlayOneShot(_clip);
    }

    private void StartPlaySoundLoop(AudioClip _clip)
    {
        if (m_audioSource == null)
            return;

        m_audioSource.clip = _clip;
        m_audioSource.Play();
    }

    private void StopPlaySoundLoop()
    {
        if (m_audioSource == null)
            return;

        m_audioSource.Stop();
    }

    private enum Movement
    {
        walk,
        run,
        sprint,
        empty,
    }

    private enum Sounds
    {
        snowfoot,
        stonefoot,
        water,
        temple,
        emtpy,
    }
}
