using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OOB
{
    public class DistanceDissolveTarget : MonoBehaviour
    {
        private OOB.Player.PlayerCharacter m_objectToTrack = null;

        private Material m_materialRef;
        private GameObject m_boundaries;
        private Material m_matRefInstance;

        private Renderer m_renderer = null;

        private void Awake()
        {
            m_boundaries = GameObject.FindWithTag("ThoraTestBoundaries");
            m_renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            TryGetPlayer();

            m_materialRef = m_renderer.material;
            m_matRefInstance = Instantiate(m_materialRef);
        }

        private OOB.Player.PlayerCharacter TryGetPlayer()
        {
            OOB.Player.PlayerCharacter player = WorldComponent.Instance?.GetPlayerCharacter();
            if (player)
            {
                m_objectToTrack = player;
                return m_objectToTrack;
            }
            else
            {
                WorldComponent.Instance.OnInitialization += () => TryGetPlayer();
                return null;
            }
        }


        private void Update()
        {
            if (m_objectToTrack != null && m_boundaries != null)
            {
                m_materialRef.SetVector("_Position", m_objectToTrack.transform.position);
            }
        }

        private void OnDestroy()
        {
            m_renderer = null;
            if (m_matRefInstance != null)
                Destroy(m_matRefInstance);

            m_matRefInstance = null;
        }
    }
}