using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOB.Enemy.Statemachine
{
    public abstract class BaseState
    {
        public BaseState (GameObject _gameObject, System.Type _nextState)
        {
            gameObject = _gameObject;
            transform = gameObject.transform;
            NextState = _nextState;
        }

        protected GameObject gameObject;
        protected Transform transform;
        protected System.Type NextState { get; set; }

        // OnStateFinsihed wrapper
        public event System.Action<System.Type> OnStateFinished;
        protected virtual void StateFinished(System.Type _newState)
        {
            OnStateFinished?.Invoke(_newState);
        }

        public abstract void FixedUpdate();
        public virtual void EnterState() { }
        public virtual void ExitState() { }
    }
}