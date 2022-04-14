using OOB.Collectible;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{

    public event System.Action OnCoinCollected;
    public event System.Action OnStarCollected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CollectibleComponent>().GetCollectibleType() == Type.Coin)
            OnCoinCollected?.Invoke();

        if (other.gameObject.GetComponent<CollectibleComponent>().GetCollectibleType() == Type.Star)
            OnStarCollected?.Invoke();
    }
}
