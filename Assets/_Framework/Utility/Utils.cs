using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class Utils
    {
        public static int GetPhysics3DLayerCollisionMask(int _layer)
        {
            int layerMask = 0;

            for (int i = 0; i < 32; i++)
            {
                if (LayerMask.LayerToName(i) == "")
                    continue;

                if (!Physics.GetIgnoreLayerCollision(_layer, i))
                {
                    layerMask |= 1 << i;
                }
            }
            return layerMask;
        }

        /// <summary>
        /// Linearly interpolates between a and b by t using a minimum and maximum for t.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation value between the two floats.</param>
        /// <param name="min">If 't' is less than 'min', 'a' will be returned.</param>
        /// <param name="max">If 't' is greater than 'max', 'b' will be returned.</param>
        /// <returns></returns>
        public static float LerpMinMax(float a, float b, float t, float min, float max)
        {
            if (t <= min)
            {
                return a;
            }
            else if (t >= max)
            {
                return b;
            }
            else
            {
                return Mathf.Lerp(a, b, (t - min) / (max - min));
            }
        }

        public static float ConvertInputIntoDegAngle(Vector2 _inputAxis)
        {
            return Vector2.SignedAngle(Vector2.up, _inputAxis);
        }

        public static float ConvertInputIntoRadAngle(Vector2 _inputAxis)
        {
            return ConvertInputIntoDegAngle(_inputAxis) * Mathf.Deg2Rad;
        }

    }
}