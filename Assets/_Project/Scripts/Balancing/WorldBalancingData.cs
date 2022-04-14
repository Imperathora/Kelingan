using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;


[CreateAssetMenu(fileName = "World Balancing", menuName = "Balancing/World Balancing")]
public class WorldBalancingData : ScriptableObject
{
    // ===== Music Fade =====

    [Header("Music Fade")]
    [SerializeField] private float m_musicFadeIn = default;
    [SerializeField] private float m_musicFadeOut = default;

    // ===== Delay For Next Level=====

    [Header("Delay time to next level")]
    [SerializeField] private float m_delayTime = default;

    public float MusicFadeIn => m_musicFadeIn;
    public float MusicFadeOut => m_musicFadeOut;
    public float DelayTime => m_delayTime;
}
