/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEngine;

namespace NatsunekoLaboratory.EnhancedTransformEditor
{
    internal class CalculateBounds : MonoBehaviour
    {
        public Vector3 Size { get; private set; }

        public void Evaluate()
        {
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var component in GetComponentsInChildren<MeshRenderer>())
            {
                var bounds = component.bounds;
                if (min.x > bounds.min.x)
                    min.x = bounds.min.x;
                if (min.y > bounds.min.y)
                    min.y = bounds.min.y;
                if (min.z > bounds.min.z)
                    min.z = bounds.min.z;
                if (max.x < bounds.max.x)
                    max.x = bounds.max.x;
                if (max.y < bounds.max.y)
                    max.y = bounds.max.y;
                if (max.z < bounds.max.z)
                    max.z = bounds.max.z;
            }

            Size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
        }
    }
}

#endif