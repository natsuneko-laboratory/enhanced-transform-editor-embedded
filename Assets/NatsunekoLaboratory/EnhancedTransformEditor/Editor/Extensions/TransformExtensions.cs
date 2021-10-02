/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEngine;

namespace NatsunekoLaboratory.EnhancedTransformEditor.Extensions
{
    internal static class TransformExtensions
    {
        public static IsolatedTransform Clone(this Transform transform)
        {
            return new IsolatedTransform
            {
                LocalPosition = transform.localPosition,
                WorldPosition = transform.position,
                LocalRotation = transform.localRotation,
                WorldRotation = transform.rotation,
                LocalScale = transform.localScale,
                WorldScale = transform.lossyScale,
                GameObject = transform.gameObject,
            };
        }
    }
}

#endif