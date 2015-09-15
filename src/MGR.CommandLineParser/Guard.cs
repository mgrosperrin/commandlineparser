using System;
using JetBrains.Annotations;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    internal static class Guard
    {
        public static void NotNull(object item, [InvokerParameterName] string name)
        {
            if (item == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void IsIConverter([NotNull] Type type, string message)
        {
            if (!typeof (IConverter).IsAssignableFrom(type))
            {
                throw new CommandLineParserException(message);
            }
        }

        public static void OfType<T>(Type type, [InvokerParameterName]  string name)
        {
            NotNull(type, name);

            if (!typeof (T).IsAssignableFrom(type))
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}