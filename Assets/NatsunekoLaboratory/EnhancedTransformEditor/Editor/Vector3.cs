/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

namespace NatsunekoLaboratory.EnhancedTransformEditor
{
    internal class Vector3<T>
    {
        public T X { get; set; }

        public T Y { get; set; }

        public T Z { get; set; }

        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
