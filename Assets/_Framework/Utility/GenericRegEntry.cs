using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public abstract class GenericRegEntry : MonoBehaviour
    {
        [SerializeField] private string m_ID = default;
        public string GetID() { return m_ID; }

        protected abstract void Initialize();
    }
}