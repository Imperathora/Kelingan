using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class ExtensionMethods
    {
        // ===== Possibly deprecated =====
        public static bool IsValid(this Object _object)
        {
            return !(_object is null);
        }

        public static bool IsValid<T>(this T _user, out T _object) where T : Object
        {
            _object = _user;
            return !(_user is null);
        }

        public static bool IsNull<T>(this T _user, out T _object) where T : Object
        {
            _object = _user;
            return (_user is null);
        }
        // ------------------------------------


        // Wants to die
        public static Collider[] GetOverlappingColliders(this SphereCollider _user)
        {
            float highestScale = _user.transform.localScale.x;

            if (highestScale < _user.transform.localScale.y)
                highestScale = _user.transform.localScale.y;

            if (highestScale < _user.transform.localScale.z)
                highestScale = _user.transform.localScale.z;

            Collider[] collider = Physics.OverlapSphere(_user.transform.position + _user.center,
                _user.radius * highestScale,
                Utils.GetPhysics3DLayerCollisionMask(_user.gameObject.layer));

            return collider;
        }

        // Wants to die, too
        public static Collider GetOverlappingCollider(this SphereCollider _user)
        {
            Collider[] coll = GetOverlappingColliders(_user);
            return coll?.Length > 0 ? coll[0] : null;
        }

        /// <summary>
        /// Turns transform towards a position.
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="_targetPos"></param>
        /// <param name="_turnAngle">Angle in radians the character is turned.</param>
        /// <param name="_yFixated"></param>
        public static void TurnTowards(this Transform _user, Vector3 _targetPos, float _turnAngle)
        {
            Vector3 newDirection = Vector3.RotateTowards(_user.forward, _targetPos - _user.position, _turnAngle, 0.0f);
            _user.rotation = Quaternion.LookRotation(newDirection);
        }

        /// <summary>
        /// Turns transform towards a position.
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="_targetPos"></param>
        /// <param name="_turnAngle"></param>
        /// <param name="_turnModifier">Modifies the turn speed depending on look direction. </param>
        public static void TurnTowards(this Transform _user, Vector3 _targetPos, float _turnAngle, float _turnModifier)
        {
            float angleDifference = Vector3.Angle(_targetPos - _user.position, _user.forward);
            _turnAngle *= (1f + (_turnModifier - 1f) * angleDifference / 180.0f);

            _user.TurnTowards(_targetPos, _turnAngle);
        }
    }
}