using OOB.Enemy;
using OOB.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageVisualization : MonoBehaviour
{
    [SerializeField] PostProcessProfile m_postProcessProfile = default;
    private Vignette m_vignetteLayer = null;
    private ChromaticAberration m_chromatic = null;
    private bool m_isFloorDamageDealt;
    private bool m_isEnemyDamageDealt;

    void Start()
    {
        m_postProcessProfile.TryGetSettings<Vignette>(out m_vignetteLayer);
        m_postProcessProfile.TryGetSettings<ChromaticAberration>(out m_chromatic);
        m_vignetteLayer.intensity.value = 0;
        m_vignetteLayer.smoothness.value = 0.4f;
        PlayerFloorDamage.OnGroundFreeze += GroundFreezeTimer;
        PlayerFloorDamage.OnDamageDealing += DealFloorDamage;
        PlayerFloorDamage.OnFadeOut += FloorFadeOut;
        OOB.Enemy.EnemyAttack.OnDamageDealing += DealEnemyDamage;
        OOB.Enemy.EnemyAttack.OnStopAttack += EnemyFadeOut;
        EnemyHealthComponent.OnFadeOut += EnemyFadeOut;
    }

    private void GroundFreezeTimer(float _timerFrozen)
    {
        m_isFloorDamageDealt = true;
        if (m_vignetteLayer.intensity.value < 1)
            m_vignetteLayer.intensity.value += Time.deltaTime / _timerFrozen * 0.7f;
        if (m_vignetteLayer.smoothness.value < 1)
            m_vignetteLayer.smoothness.value += Time.deltaTime / _timerFrozen / 0.4f;
    }

    private void DealFloorDamage(float _timerDamage)
    {
        m_isFloorDamageDealt = true;

        m_chromatic.intensity.value = 1;
    }

    private void FloorFadeOut()
    {
        m_isFloorDamageDealt = false;
        if (!m_isEnemyDamageDealt)
        {
            m_chromatic.intensity.value = 0;

            if (m_vignetteLayer.intensity.value > 0)
                m_vignetteLayer.intensity.value -= Time.deltaTime;

            if (m_vignetteLayer.smoothness.value > 0.4f)
                m_vignetteLayer.smoothness.value -= Time.deltaTime;

            if (m_vignetteLayer.smoothness.value < 0.4f)
                m_vignetteLayer.smoothness.value = 0.4f;

            if (m_vignetteLayer.intensity.value < 0)
            {
                m_vignetteLayer.intensity.value = 0;
            }
        }
    }

    private void DealEnemyDamage()
    {
        m_isEnemyDamageDealt = true;

        m_chromatic.intensity.value = 1;
    }

    private void EnemyFadeOut()
    {
        m_isEnemyDamageDealt = false;
        if (!m_isFloorDamageDealt)
        {
            m_chromatic.intensity.value = 0;
        }
    }
}
