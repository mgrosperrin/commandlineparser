using System.Globalization;
using System.Linq;
using System.Text;
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
        public static int IndexOf(this string source, params char[] values)
        {
            Guard.NotNull(source, nameof(source));

            var firstIndex = values.Select(c => source.IndexOf(c))
                .FirstOrDefault(index => index >= 0);
            return firstIndex;
        }

        public static string AsKebabCase(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            var builder = new StringBuilder();
            builder.Append(char.ToLower(source.First(), CultureInfo.CurrentUICulture));
            var introduceDash = false;
            foreach (var c in source.Skip(1))
            {
                if (char.IsUpper(c))
                {
                    if (introduceDash)
                    {
                        builder.Append('-');
                        introduceDash = false;
                    }
                    builder.Append(char.ToLower(c, CultureInfo.CurrentUICulture));
                }
                else
                {
                    introduceDash = true;
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}