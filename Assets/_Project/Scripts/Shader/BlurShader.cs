using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB
{

    public class BlurShader : MonoBehaviour
    {
        public OOB.Player.PlayerCharacter m_player = null;
        private Material m_MaterialRef = null;
        private Renderer m_Renderer = null;

        public GameObject m_BlurWall = null;
        public GameObject m_gameObject = null;

        private Material m_NewMat = null;

        public Renderer Renderer
        {
            get
            {
                if (m_Renderer == null)
                    m_Renderer = this.GetComponent<Renderer>();

                return m_Renderer;
            }
        }

        public Material MaterialRef
        {
            get
            {
                if (m_MaterialRef == null)
                {
                    m_MaterialRef = Renderer.material;
                }
                return m_MaterialRef;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            m_NewMat = Resources.Load<Material>("Blur_Material");
            TryGetPlayer();
            m_Renderer = this.GetComponent<Renderer>();
            m_MaterialRef = m_Renderer.material;

            m_BlurWall = GameObject.FindWithTag("ThoraTestBoundaries");

        }

        // Update is called once per frame
        void Update()
        {

            CheckCameraDirection();

        }

        private OOB.Player.PlayerCharacter TryGetPlayer()
        {
            OOB.Player.PlayerCharacter player = WorldComponent.Instance?.GetPlayerCharacter();
            if (player)
            {
                m_player = player;
                return m_player;
            }
            else
            {
                WorldComponent.Instance.OnInitialization += () => TryGetPlayer();
                return null;
            }
        }

        private void OnDestroy()
        {
            m_Renderer = null;
            if (m_MaterialRef != null)
                Destroy(m_MaterialRef);

            m_MaterialRef = null;
        }

        void CheckCameraDirection()
        {
            if (m_NewMat.name == m_BlurWall.GetComponent<Renderer>().material.name)
            {
                Debug.Log("if1");
                Vector3 m_cameraDirection = Camera.main.transform.forward;
                Vector3 m_MaterialPosition = m_BlurWall.transform.position;
                if(m_cameraDirection == m_MaterialPosition)
                {
                    Debug.Log("hit");
                    m_MaterialRef.SetVector("_BlueValue", Vector4.zero);
                }
            }
        }
    }
}
