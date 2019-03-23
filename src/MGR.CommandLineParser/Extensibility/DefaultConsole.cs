using System;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// Forward to <see cref="Console"/>.
    /// </summary>
    public sealed class DefaultConsole : IConsole
    {
        private readonly TextWriter _consoleOut;
        private readonly TextWriter _consoleError;

        /// <summary>
        /// Create a new <see cref="DefaultConsole"/>.
        /// </summary>
        [PublicAPI]
        public DefaultConsole() : this(Console.Out, Console.Error) { }

        internal DefaultConsole(TextWriter consoleOut, TextWriter consoleError)
        {
            _consoleOut = consoleOut;
            _consoleError = consoleError;
        }

        /// <inheritdoc />
        public void Write(string format, params object[] args)
        {
            WriteColor(_consoleOut, Console.ForegroundColor, format, args);
        }

        /// <inheritdoc />
        public void WriteError(string format, params object[] args)
        {
            WriteColor(_consoleError, ConsoleColor.Red, format, args);
        }

        /// <inheritdoc />
        public void WriteWarning(string format, params object[] args)
        {
            var message = string.Format(CultureInfo.CurrentUICulture, Strings.Console_WarningFormat, format);
            WriteColor(_consoleOut, ConsoleColor.Yellow, message, args);
        }

        private static void WriteColor(TextWriter writer, ConsoleColor color, string value, params object[] args)
        {
            var currentColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                writer.Write(value, args);
            }
            finally
            {
                Console.ForegroundColor = currentColor;
            }
        }
    }
}