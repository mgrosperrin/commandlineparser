using System;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser
{
    internal static class Guard
    {
        internal static void NotNull(object item, [InvokerParameterName] string name)
        {
            if (item == null)
            {
                throw new ArgumentNullException(name);
            }
        }
        internal static void NotNullOrEmpty(string item, [InvokerParameterName] string name)
        {
            if (string.IsNullOrEmpty(item))
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void IsIConverter([NotNull] Type type, string message)
        {
            if (!typeof (IConverter).IsAssignableFrom(type))
            {
                throw new CommandLineParserException(message);
            }
        }

        internal static void OfType<T>(Type type, [InvokerParameterName]  string name)
        {
            NotNull(type, name);

            if (!typeof (T).IsAssignableFrom(type))
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}