using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Player;
using OOB.Sectors;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private GameObject m_shadowContainer = default;
    private Transform m_playerTransform;
    private Vector3 m_parentOffset = new Vector3(0f, 0.01f, 0f);
    private SectorTracker m_sectorTracker;
    private CharacterWorldMovement m_worldMovement;
    private float m_timer;

    private void Start()
    {
        m_playerTransform = OOB.WorldComponent.Instance?.GetPlayerCharacter()?.transform;
        m_sectorTracker = OOB.WorldComponent.Instance.GetPlayerCharacter().GetSectorTracker();
        m_worldMovement = OOB.WorldComponent.Instance.GetPlayerCharacter().GetCharacterWorldMovement();

        if (m_playerTransform == null)
        {
            OOB.WorldComponent.Instance.OnInitialization += () => { m_playerTransform = OOB.WorldComponent.Instance.GetPlayerCharacter().transform; };
        }
    }


    private void Update()
    {


        if (m_sectorTracker.IsInWorld)
        {
            if (!m_worldMovement.IsPlayerGrounded() && !m_worldMovement.IsPlayerJumping())
            {
                m_timer += Time.deltaTime;

                if (m_timer > 0.4f)
                {
                    m_shadowContainer.GetComponent<Renderer>().enabled = false;
                    return;
                }


            } else
            {
                m_timer = 0;
                m_shadowContainer.GetComponent<Renderer>().enabled = true;
            }


            if (m_playerTransform)
            {
                Ray ray = new Ray(m_playerTransform.position, -Vector3.up);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 100f))
                {

                    if (hitInfo.distance > 0)
                    {
                        Vector3 newScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.zero, hitInfo.distance / 12);
                        transform.localScale = newScale;
                    }



                    m_shadowContainer.transform.position = hitInfo.point + m_parentOffset;

                    //Set shadow align to surface
                    //transform.up = hitInfo.normal;

                }

            }

        }
        else
        {
            m_shadowContainer.GetComponent<Renderer>().enabled = false;
        }

    }


}
