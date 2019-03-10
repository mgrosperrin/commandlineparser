﻿
// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    internal static class EnumeratorExtensions
    {
        internal static string GetNextCommandLineItem(this IEnumerator<string> argsEnumerator)
        {
            if (argsEnumerator == null || !argsEnumerator.MoveNext())
            {
                return null;
            }
            return argsEnumerator.Current;
        }

        internal static IEnumerator<string> PrefixWith(this IEnumerator<string> argsEnumerator, string prefix)
        {
            var list = new List<string> {prefix};
            while (argsEnumerator != null && argsEnumerator.MoveNext())
            {
                list.Add(argsEnumerator.Current);
            }
            return list.GetEnumerator();
        }
    }
}