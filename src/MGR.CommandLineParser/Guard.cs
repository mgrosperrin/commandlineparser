using System;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    internal static class Guard
    {
        public static void NotNull(object item, string name)
        {
            if (item == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void IsIConverter(Type type, string message)
        {
            if (!typeof(IConverter).IsAssignableFrom(type))
            {
                throw new CommandLineParserException(message);
            }
        }
    }
}