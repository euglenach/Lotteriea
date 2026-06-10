using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LotterySystem
{
    internal static class InternalUtility
    {
        internal static bool TryAsSpan<T>(IEnumerable<T> source, out Span<T> span)
        {
            if(source is T[] array)
            {
                span = array.AsSpan();
                return true;
            }
            else if(source is List<T> list)
            {
                span = AsSpan(list);
                return true;
            }
        
            span = Span<T>.Empty;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Span<T> AsSpan<T>(List<T> self)
        {
#if NET7_0_OR_GREATER
            return System.Runtime.InteropServices.CollectionsMarshal.AsSpan(self);
#else
            return Unsafe.As<ListDummy<T>>(self).Items.AsSpan(0, self.Count);
#endif
        }

        /// <summary>
        /// ListのAsSpan用。
        /// </summary>
        private class ListDummy<T> { internal T[] Items; }
    }
}