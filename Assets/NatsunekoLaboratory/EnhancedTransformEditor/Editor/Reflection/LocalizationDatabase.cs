/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Reflection;

using NatsunekoLaboratory.EnhancedTransformEditor.Reflection.Expressions;

using UnityEditorInternal;

namespace NatsunekoLaboratory.EnhancedTransformEditor.Reflection
{
    internal static class LocalizationDatabase
    {
        private static readonly Type T;

        static LocalizationDatabase()
        {
            T = typeof(AssetStore).Assembly.GetType("UnityEditor.LocalizationDatabase");
        }

        public static string GetLocalizedString(string original)
        {
            return ReflectionStaticClass.InvokeMethod<string>(T, nameof(GetLocalizedString), BindingFlags.Public | BindingFlags.Static, original);
        }
    }
}

#endif