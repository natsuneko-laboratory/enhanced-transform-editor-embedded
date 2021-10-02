/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Reflection;

using NatsunekoLaboratory.EnhancedTransformEditor.Reflection.Expressions;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace NatsunekoLaboratory.EnhancedTransformEditor.Reflection
{
    // ReSharper disable once InconsistentNaming
    internal class TransformRotationGUI : ReflectionClass
    {
        private static readonly Type T;

        static TransformRotationGUI()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditor.TransformRotationGUI");
        }

        private TransformRotationGUI(object instance) : base(instance, T) { }

        public TransformRotationGUI() : this(Activator.CreateInstance(T)) { }

        public void OnEnable(SerializedProperty a, GUIContent b)
        {
            InvokeMethod(nameof(OnEnable), BindingFlags.Public | BindingFlags.Instance, a, b);
        }

        public void RotationField()
        {
            InvokeMethodStrict(nameof(RotationField), BindingFlags.Public | BindingFlags.Instance, new StrictParameter { Type = typeof(bool), Value = false });
        }
    }
}

#endif