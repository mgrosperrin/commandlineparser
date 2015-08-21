using System;

// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser
    // ReSharper restore CheckNamespace
{
    internal static class ParserOptionsExtensions
    {
        internal static IParserOptions AsReadOnly(this IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            return new ReadOnlyParserOptions(options);
        }
    }
}