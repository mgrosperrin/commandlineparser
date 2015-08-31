using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class StringExtensions
    {
        public static bool StartsWith(this string source, StringComparison comparisonType, params string[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return values.Any(value => source.StartsWith(value, comparisonType));
        }
    }
}