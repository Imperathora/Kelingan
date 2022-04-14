using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuComponent : MonoBehaviour
{
    [SerializeField] protected UnityEvent OnMenuEnabled;
    [SerializeField] protected UnityEvent OnMenuDisabled;


    public virtual void EnableMenu()
    {
        OnMenuEnabled.Invoke();
    }

    public virtual void DisableMenu()
    {
        OnMenuDisabled.Invoke();
    }

}
