using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Framework.Data;
using Framework.Events;

namespace Framework.Input
{
    [CreateAssetMenu(fileName = "Input Handler", menuName = "Events/Input Handler")]
    public class InputHandler : ScriptableObject
    {
        public GameEvent OnInput;
        public List<SingleInputAction> m_singleInputActions = new List<SingleInputAction>(0);
        public List<DoubleInputAction> m_doubleInputActions = new List<DoubleInputAction>(0);
        public List<AxisInputAction> m_axisInputActions = new List<AxisInputAction>(0);

        private GenericLock m_inputLock = new GenericLock();

        public bool IsInputEnabled
        {
            get
            {
                return m_inputLock.IsUnlocked;
            }
        }

        public List<GameEvent> m_cachedGameEvents { get; private set; } = new List<GameEvent>();

        public bool Initialize(Player _rewiredPlayer)
        {
            foreach (SingleInputAction a in m_singleInputActions)
            {
                a.Initialize(_rewiredPlayer, this);
            }

            foreach (DoubleInputAction a in m_doubleInputActions)
            {
                a.Initialize(_rewiredPlayer, this);
            }

            foreach (AxisInputAction a in m_axisInputActions)
            {
                a.Initialize(_rewiredPlayer, this);
            }

            return true;
        }

        public void EnableInput(Object _enabler)
        {
            m_inputLock.Unlock(_enabler);
            CatchInput();
        }

        public void DisableInput(Object _disabler)
        {
            m_inputLock.Lock(_disabler);
        }

        public void ClearInput()
        {
            m_inputLock.Clear();
        }

        private void CatchInput()
        {
            foreach (GameEvent e in m_cachedGameEvents)
            {
                e.Invoke();
            }

            m_cachedGameEvents.Clear();
        }

        public void AddAxisInputAction()
        {
            m_axisInputActions.Add(new AxisInputAction());
        }

        public void AddSingleInputAction()
        {
            m_singleInputActions.Add(new SingleInputAction());
        }

        public void AddDoubleInputAction()
        {
            m_doubleInputActions.Add(new DoubleInputAction());
        }

        private void OnEnable()
        {
            m_inputLock.Clear();
        }
    }

    [System.Serializable]
    public class SingleInputAction
    {
        public string m_actionID;
        public GameEvent OnButtonPressed;

        private Player m_rewiredPlayer;
        private InputHandler m_inputHandler;

        ~SingleInputAction()
        {
            m_rewiredPlayer.RemoveInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, m_actionID);
        }

        public void Initialize(Player _rewiredPlayer, InputHandler _inputHandler)
        {
            m_rewiredPlayer = _rewiredPlayer;
            m_inputHandler = _inputHandler;
            m_rewiredPlayer.AddInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, m_actionID);
        }

        protected virtual void ButtonPressed(InputActionEventData _data)
        {
            if (m_inputHandler.IsInputEnabled)
            {
                m_inputHandler.OnInput.Invoke();
                OnButtonPressed.Invoke();
            }
        }
    }

    [System.Serializable]
    public class DoubleInputAction
    {
        public string m_actionID;
        public GameEvent OnButtonPressed;
        public GameEvent OnButtonReleased;

        private Player m_rewiredPlayer;
        private InputHandler m_inputHandler;

        private bool m_isButtonDown;

        ~DoubleInputAction()
        {
            m_rewiredPlayer.RemoveInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, m_actionID);
            m_rewiredPlayer.RemoveInputEventDelegate(ButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, m_actionID);
        }

        public void Initialize(Player _rewiredPlayer, InputHandler _inputHandler)
        {
            m_rewiredPlayer = _rewiredPlayer;
            m_inputHandler = _inputHandler;
            m_rewiredPlayer.AddInputEventDelegate(ButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, m_actionID);
            m_rewiredPlayer.AddInputEventDelegate(ButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, m_actionID);
        }

        private void ButtonPressed(InputActionEventData _data)
        {
            if (m_inputHandler.IsInputEnabled)
            {
                m_inputHandler.OnInput.Invoke();
                OnButtonPressed.Invoke();
                m_isButtonDown = true;
            }
            else if (m_inputHandler.m_cachedGameEvents.Contains(OnButtonReleased))
            {
                m_inputHandler.m_cachedGameEvents.Remove(OnButtonReleased);
                m_isButtonDown = true;
            }
        }

        private void ButtonReleased(InputActionEventData _data)
        {
            if (m_inputHandler.IsInputEnabled)
            {
                m_inputHandler.OnInput.Invoke();
                OnButtonReleased.Invoke();
                m_isButtonDown = false;
            }
            else if (m_isButtonDown)
            {
                m_isButtonDown = false;
                m_inputHandler.m_cachedGameEvents.Add(OnButtonReleased);
            }
        }
    }

    [System.Serializable]
    public class AxisInputAction
    {
        public string m_actionID;
        public FloatContainer m_inputAxis;

        private Player m_rewiredPlayer;
        private InputHandler m_inputHandler;

        ~AxisInputAction()
        {
            m_rewiredPlayer.RemoveInputEventDelegate(AxisInput, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, m_actionID);
        }

        public void Initialize(Player _rewiredPlayer, InputHandler _inputHandler)
        {
            m_rewiredPlayer = _rewiredPlayer;
            m_inputHandler = _inputHandler;
            m_rewiredPlayer.AddInputEventDelegate(AxisInput, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, m_actionID);
        }

        private void AxisInput(InputActionEventData _data)
        {
            if (m_inputHandler.IsInputEnabled)
            {
                m_inputHandler.OnInput.Invoke();
                m_inputAxis.SetValue(_data.GetAxis());
            }
            else
            {
                m_inputAxis.SetValue(0.0f);
            }
        }
    }
}