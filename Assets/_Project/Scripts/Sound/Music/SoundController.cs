using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB;
using OOB.Player;
using OOB.Sectors;
using System;

public class SoundController : MonoBehaviour
{
    [SerializeField] private WorldBalancingData m_worldBalancingData = default;
    [SerializeField] private AudioClip m_inboundMusic;
    [SerializeField] private AudioClip m_outboundMusic;
    [SerializeField] private AudioClip m_windSound;
    [SerializeField] private AudioSource m_inboundAudioSourceStart;
    [SerializeField] private AudioSource m_outboundAudioSourceStart;
    [SerializeField] private AudioSource m_ambienceSound;
    [SerializeField] private AudioSource m_audioSourceLooperOne;
    [SerializeField] private AudioSource m_audioSourceLooperTwo;
    [SerializeField] private Portal m_portal;


    public static SoundController Instance;
    private PlayerCharacter m_player;
    private AmbientSoundTrigger m_ambientTrigger;
    private float m_standardSoundVolume;
    private float m_ambienceVolume;
    private bool m_inboundStart;
    private bool m_outboundStart;
    private bool m_looperOne;
    private bool m_looperTwo;
    private bool m_ambience;
    private float m_currentVolume;
    private bool m_isFirstStart = true;
    private bool m_isFadingOut;
    private bool m_isFadingIn;
    private bool m_playerIsRespwaning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TryGetPlayer();


        m_audioSourceLooperOne.loop = true;
        m_audioSourceLooperTwo.loop = true;

        m_standardSoundVolume = m_inboundAudioSourceStart.volume;

        m_ambienceSound.clip = m_windSound;
        m_ambienceSound.Play();

        m_ambienceVolume = m_ambienceSound.volume;
        m_portal.OnEnterPortal += StopAll;
    }

    private void TryGetPlayer()
    {

        m_player = WorldComponent.Instance?.GetPlayerCharacter();

        if (m_player)
        {
            m_ambientTrigger = m_player.GetComponentInChildren<AmbientSoundTrigger>();

            m_player.OnPlayerDeath += StopAll;
            m_player.OnPlayerSpawn += StartMusic;

            m_player.OnPlayerIsInVoidStop += () => PlayMusic(m_inboundMusic, m_inboundAudioSourceStart, m_outboundAudioSourceStart, false);
            m_player.OnPlayerIsInVoidStart += () => PlayMusic(m_outboundMusic, m_outboundAudioSourceStart, m_inboundAudioSourceStart, false);

            m_player.OnPlayerIsInVoidStopAmbience += () => StartCoroutine(AmbienceFadeOut(m_ambienceSound, 2f));
            m_player.OnPlayIsInVoidStartAmbience += () => StartCoroutine(AmbienceFadeIn(m_ambienceSound, 2f));

            m_ambientTrigger.OnWindFadeOut += () => StartCoroutine(AmbienceFadeOut(m_ambienceSound, 2f));
            m_ambientTrigger.OnWindFadeIn += () => StartCoroutine(AmbienceFadeIn(m_ambienceSound, 2f));


        }
        else
        {
           WorldComponent.Instance.OnInitialization += TryGetPlayer;
        }
    }
  
    private void Update()
    {
        /////////////////////////////////////// From Start To Looper ///////////////////////////////////
        if (m_inboundStart)
        {
            float length = m_inboundAudioSourceStart.clip.length - 3f;

            m_standardSoundVolume = m_inboundAudioSourceStart.volume;

            if (m_inboundAudioSourceStart.time > length)
            {
                if (m_inboundAudioSourceStart.clip.name == m_inboundMusic.name)
                {
                    PlayMusic(m_inboundMusic, m_audioSourceLooperOne, m_inboundAudioSourceStart, true);
                } else
                {
                    PlayMusic(m_outboundMusic, m_audioSourceLooperOne, m_inboundAudioSourceStart, true);
                }
            }
        }

        if (m_outboundStart)
        {
            float length = m_outboundAudioSourceStart.clip.length - 3f;

            m_standardSoundVolume = m_outboundAudioSourceStart.volume;
            if (m_outboundAudioSourceStart.time > length)
            {
                if(m_outboundAudioSourceStart.clip.name == m_outboundMusic.name)
                {
                    PlayMusic(m_outboundMusic, m_audioSourceLooperOne, m_outboundAudioSourceStart, true);
                } else
                {
                    PlayMusic(m_inboundMusic, m_audioSourceLooperOne, m_outboundAudioSourceStart, true);
                }
            }
        }

        /////////////////////////////////////// From Looper To Looper ////////////////////////////////

        if (m_looperOne)
        {
            float length = m_audioSourceLooperOne.clip.length - 3f;

            m_standardSoundVolume = m_audioSourceLooperOne.volume;
            if (m_audioSourceLooperOne.time > length)
            {
                if (m_audioSourceLooperOne.clip.name == m_inboundMusic.name)
                {
                    PlayMusic(m_inboundMusic, m_audioSourceLooperTwo, m_audioSourceLooperOne, true);
                } else
                {
                    PlayMusic(m_outboundMusic, m_audioSourceLooperTwo, m_audioSourceLooperOne, true);
                }
            }
        }

        if (m_looperTwo)
        {
            float length = m_audioSourceLooperTwo.clip.length - 3f;

            m_standardSoundVolume = m_audioSourceLooperTwo.volume;
            if (m_audioSourceLooperTwo.time > length)
            {
                if (m_audioSourceLooperTwo.clip.name == m_inboundMusic.name)
                {
                    PlayMusic(m_inboundMusic, m_audioSourceLooperOne, m_audioSourceLooperTwo, true);
                }
                else
                {
                    PlayMusic(m_outboundMusic, m_audioSourceLooperOne, m_audioSourceLooperTwo, true);
                }
            }
        }
    }



    private IEnumerator FadeOutLooper(AudioSource _audioSource, float _duration)
    {
        float currentTime = 0;
        float start = _audioSource.volume;

        while (currentTime < _duration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, 0, currentTime / _duration);
            if (_audioSource.volume < 0.1f)
                _audioSource.Stop();

                

            yield return null;
        }
    }

    private IEnumerator FadeInLooper(AudioSource _audioSource, float _duration, AudioClip _clip, float _volume)
    {
        float currentTime = 0;
        if (_audioSource == m_audioSourceLooperOne)
        {
            m_looperOne = true;
            m_looperTwo = false;
            m_inboundStart = false;
            m_outboundStart = false;
        } else
        {
            m_looperTwo = true;
            m_looperOne = false;
            m_inboundStart = false;
            m_outboundStart = false;
        }
            

            _audioSource.clip = _clip;
            _audioSource.Play();


        while (currentTime < _duration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0, _volume, currentTime / _duration);

            yield return null;
        }
    }


    private void PlayMusic(AudioClip _musicIn, AudioSource _audioSourceIn, AudioSource _audioSourceOut, bool _looper)
    {

        if (_looper)
        {
            StartCoroutine(FadeInLooper(_audioSourceIn, m_worldBalancingData.MusicFadeIn, _musicIn, m_standardSoundVolume));
            StartCoroutine(FadeOutLooper(_audioSourceOut, m_worldBalancingData.MusicFadeOut));
        } else
        {
            StartCoroutine(FadeIn(_audioSourceIn, m_worldBalancingData.MusicFadeIn, _musicIn, m_standardSoundVolume));
            StartCoroutine(FadeOut(_audioSourceOut, m_worldBalancingData.MusicFadeOut));

            StartCoroutine(MuteLooper());
        }
    }

    public void StopAll()
    {
        m_playerIsRespwaning = true;
        m_inboundAudioSourceStart.Stop();
        m_outboundAudioSourceStart.Stop();
        m_ambienceSound.Stop();
        m_audioSourceLooperOne.Stop();
        m_audioSourceLooperTwo.Stop();
        m_ambienceSound.Stop();
    }

    private void StartMusic()
    {
        //Music
        m_inboundAudioSourceStart.volume = m_standardSoundVolume;
        m_inboundAudioSourceStart.clip = m_inboundMusic;
        m_inboundAudioSourceStart.Play();

        //Ambience
        m_ambienceSound.volume = m_ambienceVolume;
        m_ambienceSound.Play();
    }

    private IEnumerator MuteLooper()
    {
        float currentTime = 0;
        float startLooperOne = m_audioSourceLooperOne.volume;
        float startLooperTwo = m_audioSourceLooperTwo.volume;

        while (currentTime < 2f)
        {
            currentTime += Time.deltaTime;
            m_audioSourceLooperOne.volume = Mathf.Lerp(startLooperOne, 0, currentTime / 2f);
            m_audioSourceLooperTwo.volume = Mathf.Lerp(startLooperTwo, 0, currentTime / 2f);

            yield return null;
        }
    }

    private IEnumerator FadeOut(AudioSource _audioSource, float _duration)
    {
        if (m_playerIsRespwaning)
        {
            m_playerIsRespwaning = false;
            yield break;
        }
            

        float currentTime = 0;
        float start = _audioSource.volume;



        while (currentTime < _duration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, 0, currentTime / _duration);

            if (_audioSource.volume == 0)
                _audioSource.Pause();

            yield return null;
        }
    }

    private IEnumerator FadeIn(AudioSource _audioSource, float _duration, AudioClip _clip, float _volume)
    {

        if (m_standardSoundVolume == _audioSource.volume && !m_isFirstStart || m_playerIsRespwaning)
            yield break;

        m_isFirstStart = false;
        float currentTime = 0;

        if (_audioSource == m_inboundAudioSourceStart)
        {
            m_inboundStart = true;
            m_outboundStart = false;
            m_looperOne = false;
            m_looperTwo = false;
        }
        else 
        {
            m_outboundStart = true;
            m_inboundStart = false;
            m_looperOne = false;
            m_looperTwo = false;
        }
    
           if (_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            else
            {
                _audioSource.clip = _clip;
                _audioSource.Play();
            }
        

        while (currentTime < _duration)
        {

            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0, _volume, currentTime / _duration);

            yield return null;
        }
    }

    private IEnumerator AmbienceFadeIn(AudioSource _audioSource, float _duration)
    {
        if (_audioSource.volume == m_ambienceVolume)
            yield break;

            

        //if (m_isFirstStart)
        //{
        //    m_isFirstStart = false;
        //    yield break;
        //}


        float currentTime = 0;

        while (currentTime < _duration)
        {

            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0, m_ambienceVolume, currentTime / _duration);

            yield return null;
        }


    }

    private IEnumerator AmbienceFadeOut(AudioSource _audioSource, float _duration)
    {
        if (_audioSource.volume == 0)
            yield break;

        float currentTime = 0;

        while (currentTime < _duration)
        {
           
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(m_ambienceVolume, 0, currentTime / _duration);
            
            yield return null;
        }
       

    }
}
