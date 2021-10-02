/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;

namespace NatsunekoLaboratory.EnhancedTransformEditor.Extensions
{
    // ReSharper disable once InconsistentNaming
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<T1> MinBy<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> selector)
        {
            return source.OrderBy(selector);
        }

        public static IEnumerable<T1> MaxBy<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> selector)
        {
            return source.OrderByDescending(selector);
        }
    }
}

#endif