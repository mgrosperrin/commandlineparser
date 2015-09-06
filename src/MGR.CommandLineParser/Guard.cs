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
    }
}