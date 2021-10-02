/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;

namespace NatsunekoLaboratory.EnhancedTransformEditor
{
    internal class Union<T1, T2>
    {
        public T1 Left { get; }

        public T2 Right { get; }

        public bool IsLeft => !IsRight;

        public bool IsRight { get; }

        public Union(T1 t)
        {
            (Left, Right, IsRight) = (t, default, false);
        }

        public Union(T2 t)
        {
            (Left, Right, IsRight) = (default, t, true);
        }

        public T1 AsT1 => IsLeft ? Left : throw new InvalidOperationException();

        public T2 AsT2 => IsRight ? Right : throw new InvalidOperationException();

        public static implicit operator Union<T1, T2>(T1 t) => new Union<T1, T2>(t);
        public static implicit operator Union<T1, T2>(T2 t) => new Union<T1, T2>(t);
    }
}

#endif