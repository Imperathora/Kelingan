using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Player;

public class Controller : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_velocity = default;
    [SerializeField] private Animator m_playerAnimator;


    private void Start()
    {
        m_playerAnimator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        Vector3 velocity = Vector3.zero;

        velocity.x += m_velocity;
        velocity.z += m_velocity;

        m_rigidbody.velocity = velocity;

        Debug.Log(velocity.magnitude);

        m_playerAnimator.SetFloat("PlayerVelocity", velocity.magnitude);
    }


}
