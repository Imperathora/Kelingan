using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    private int m_layer;
    public int GetColliderLayer() => m_layer;
    private void OnTriggerStay(Collider other)
    {
          if(other.isTrigger)
            m_layer = other.gameObject.layer;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            m_layer = -1;
    }
}
