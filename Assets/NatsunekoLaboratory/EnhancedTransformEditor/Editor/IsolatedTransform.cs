/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEngine;

namespace NatsunekoLaboratory.EnhancedTransformEditor
{
    internal class IsolatedTransform
    {
        public Vector3 LocalPosition { get; set; }

        public Vector3 WorldPosition { get; set; }

        public Quaternion LocalRotation { get; set; }

        public Quaternion WorldRotation { get; set; }

        public Vector3 LocalScale { get; set; }

        public Vector3 WorldScale { get; set; }

        public GameObject GameObject { get; set; }
    }
}

#endif