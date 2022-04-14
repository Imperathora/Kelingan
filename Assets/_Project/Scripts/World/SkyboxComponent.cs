using OOB;
using OOB.Sectors;
using OOB.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxComponent : MonoBehaviour
{

    [SerializeField] private Material m_skyboxLevel;
    [SerializeField] private Material m_skyboxVoid;
    private SectorTracker m_sectorTracker;

    private void Start()
    {
        TryGetPlayer();
    }

    private void TryGetPlayer()
    {

        PlayerCharacter player = WorldComponent.Instance?.GetPlayerCharacter();
        

        if (player) 
        { 
            m_sectorTracker = player.GetSectorTracker();
            m_sectorTracker.OnWorldExit += ChangeSkyboxMaterialToVoid;
            m_sectorTracker.OnWorldEnter += ChangeSkyboxMaterialToWorld;
        }
        else
        {
            WorldComponent.Instance.OnInitialization += TryGetPlayer;
        }
    }
    private void OnDestroy()
    {
        m_sectorTracker.OnWorldExit -= ChangeSkyboxMaterialToVoid;
        m_sectorTracker.OnWorldEnter -= ChangeSkyboxMaterialToWorld;
    }

    private void ChangeSkyboxMaterialToVoid()
    {
        RenderSettings.skybox = m_skyboxVoid;
    }

    private void ChangeSkyboxMaterialToWorld()
    {
        RenderSettings.skybox = m_skyboxLevel;
    }

}
