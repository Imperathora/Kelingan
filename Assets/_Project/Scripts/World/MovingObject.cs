using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB;
using OOB.Player;


public class MovingObject : MonoBehaviour
{
    private PlayerCharacter m_playerCharacter;
    //private Animator ani;

    //private void Start()
    //{
    //    ani = GetComponent<Animator>();
    //}

    private void OnTriggerEnter(Collider other)
    {
       
        m_playerCharacter = WorldComponent.Instance?.GetPlayerCharacter();

        if (m_playerCharacter == null)
            return;


        if (other.gameObject == m_playerCharacter.gameObject)
        {
            m_playerCharacter.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            m_playerCharacter.gameObject.transform.parent = transform;
        }
            

        //ani.Update(Time.deltaTime);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_playerCharacter.gameObject)
        {
            m_playerCharacter.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            m_playerCharacter.gameObject.transform.parent = null;
        }
            
    }

}
