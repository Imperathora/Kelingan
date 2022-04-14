using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 _source, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? _source.x, y ?? _source.y, z ?? _source.z);
        }

        public static Vector3 DirectionTo(this Vector3 _source, Vector3 _destination)
        {
            return Vector3.Normalize(_destination - _source);
        }

        public static Vector3 DirectionTo(this Vector3 _source, Transform _destination)
        {
            return Vector3.Normalize(_destination.position - _source);
        }
    }
}