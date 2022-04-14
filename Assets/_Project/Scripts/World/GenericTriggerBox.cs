using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerBox : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTriggerIn = default;
    [SerializeField] private UnityEvent OnTriggerOut = default;


    private void OnTriggerEnter(Collider other)
    {
        OnTriggerIn?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerOut?.Invoke();
    }
}
