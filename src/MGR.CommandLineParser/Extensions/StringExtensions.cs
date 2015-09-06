using System.Linq;
using MGR.CommandLineParser;

// ReSharper disable once CheckNamespace

namespace System
{
    internal static class StringExtensions
    {
        public static bool StartsWith(this string source, StringComparison comparisonType, params string[] values)
        {
            Guard.NotNull(source, nameof(source));

            return values.Any(value => source.StartsWith(value, comparisonType));
        }
    }
}