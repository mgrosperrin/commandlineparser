
// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    internal static class EnumeratorExtensions
    {
        public static string GetNextCommandLineItem(this IEnumerator<string> argsEnumerator)
        {
            if (argsEnumerator == null || !argsEnumerator.MoveNext())
            {
                return null;
            }
            return argsEnumerator.Current;
        }
    }
}