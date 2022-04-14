using UnityEngine;
using OOB.Player;
using OOB;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerFloorDamage : MonoBehaviour
{
    [SerializeField] float m_timeTillDamage = default;
    [SerializeField] float m_timeTillFrozen = default;

    private float m_resetTimeDamage;
    private float m_resetTimeFrozen;

    public static System.Action<float> OnGroundFreeze;
    public static System.Action<float> OnDamageDealing;
    public static System.Action OnFadeOut;

    private OOB.Player.PlayerCharacter m_player = null;

    private CharacterWorldMovement m_characterMovement;

    public static System.Action OnDamageMaterial;

    private bool m_isFreezeTimer = false;


    private void Start()
    {
        TryGetPlayer();
        m_characterMovement = m_player.GetCharacterWorldMovement();
        m_resetTimeDamage = m_timeTillDamage;
        m_resetTimeFrozen = m_timeTillFrozen;

    }

    private void Update()
    {
        if (IsDamageTag())
        {
            if (m_characterMovement.IsPlayerGrounded())
            {
                OnGroundFreeze.Invoke(m_resetTimeFrozen);

                m_timeTillFrozen -= Time.deltaTime;

                if (m_timeTillFrozen < 0)
                {
                    if (m_timeTillFrozen <= 0 && m_timeTillDamage == m_resetTimeDamage && !m_isFreezeTimer)
                    {
                        OnDamageMaterial.Invoke();
                        m_isFreezeTimer = true;
                    }

                    if (m_isFreezeTimer)
                    {
                        m_timeTillFrozen = 0;
                        OnDamageDealing.Invoke(m_resetTimeDamage);
                        m_timeTillDamage -= Time.deltaTime;
                    }

                }
            }

            if (m_timeTillDamage <= 0)
            {
                OnDamageMaterial.Invoke();
                m_timeTillDamage = m_resetTimeDamage;
            }
        }

        if (!IsDamageTag() && m_characterMovement.IsPlayerGrounded())
        {
            m_timeTillDamage = m_resetTimeDamage;
            m_timeTillFrozen = m_resetTimeFrozen;
            m_isFreezeTimer = false;
            OnFadeOut.Invoke();
        }

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

    private bool IsDamageTag()
    {
        bool collided = false;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(GetComponent<CapsuleCollider>().bounds.center + transform.forward * GetComponent<CapsuleCollider>().radius, Vector3.down, out hit, GetComponent<CapsuleCollider>().height / 2 + 4f))
        {
            collided = false;
            if (hit.collider.gameObject.tag == "Ice")
                collided = true;
        }
        else
            collided = false;

        return collided;
    }
}
