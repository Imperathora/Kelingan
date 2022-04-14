using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace OOB.Player
{
    public abstract class PlayerMovementBase : MonoBehaviour
    {
        protected Rigidbody m_rigidbody;

        // Combines camera direction and input direction into the direction the character should look into with the y-axis.
        protected Vector3 CalcLookDirection(Vector2 _inputAxis, Vector3 _cameraDirection)
        {
            float inputAngle = Utils.ConvertInputIntoRadAngle(_inputAxis);

            return new Vector3(
            Mathf.Cos(inputAngle) * _cameraDirection.x - Mathf.Sin(inputAngle) * _cameraDirection.z,
            Mathf.Cos(inputAngle) * _cameraDirection.y,
            Mathf.Sin(inputAngle) * _cameraDirection.x + Mathf.Cos(inputAngle) * _cameraDirection.z);
        }

        // Combines camera direction and input direction into the direction the character should look into without the y-axis.
        protected Vector3 CalcLookDirectionFlat(Vector2 _inputAxis, Vector3 _cameraDirection)
        {
            return CalcLookDirection(_inputAxis, _cameraDirection).With(y: 0f);
        }

        // Turn rigidbody towards new direction depending on turn mode.
        protected void TurnTowards(Vector3 _currentDirection, Vector3 _newDirection, TurnMode _turnMode, float _turnSpeed)
        {

            if (_turnMode == TurnMode.Instant)
            {
                m_rigidbody.rotation = Quaternion.LookRotation(_newDirection);
            }
            else if (_turnMode == TurnMode.Interpolation)
            {
                _newDirection = Vector3.RotateTowards(
                    _currentDirection, _newDirection, _turnSpeed * Time.fixedDeltaTime, 0f);

                m_rigidbody.MoveRotation(Quaternion.LookRotation(_newDirection));
            }
        
        }
    }
}