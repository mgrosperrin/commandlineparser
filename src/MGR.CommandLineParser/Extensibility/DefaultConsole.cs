using System;
using System.Globalization;
using System.IO;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// Forward to <see cref="Console"/>.
    /// </summary>
    public sealed class DefaultConsole : IConsole
    {
        internal static readonly IConsole Instance = new DefaultConsole();
        private readonly TextWriter _consoleOut;
        private readonly TextWriter _consoleError;

        internal DefaultConsole() : this(Console.Out, Console.Error) { }

        internal DefaultConsole(TextWriter consoleOut, TextWriter consoleError)
        {
            _consoleOut = consoleOut;
            _consoleError = consoleError;
        }

        /// <inheritdoc />
        public void Write(string format, params object[] args)
        {
            WriteDefaultColor(_consoleOut, format, args);
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            WriteLineDefaultColor(_consoleOut, "");
        }

        /// <inheritdoc />
        public void WriteLine(string value)
        {
            WriteLineDefaultColor(_consoleOut, value);
        }

        /// <inheritdoc />
        public void WriteLine(string format, params object[] args)
        {
            WriteLineDefaultColor(_consoleOut, format, args);
        }

        /// <inheritdoc />
        public void WriteError(string value)
        {
            WriteError(value, new object[0]);
        }

        /// <inheritdoc />
        public void WriteError(string format, params object[] args)
        {
            WriteColor(_consoleError, ConsoleColor.Red, format, args);
        }

        /// <inheritdoc />
        public void WriteWarning(string value)
        {
            WriteWarning(value, new object[0]);
        }

        /// <inheritdoc />
        public void WriteWarning(string format, params object[] args)
        {
            var message = string.Format(CultureInfo.CurrentUICulture, Strings.Console_WarningFormat, format);
            WriteColor(_consoleOut, ConsoleColor.Yellow, message, args);
        }

        private static void WriteLineDefaultColor(TextWriter writer, string value, params object[] args)
        {
            WriteLineColor(writer, Console.ForegroundColor, value, args);
        }

        private static void WriteDefaultColor(TextWriter writer, string value, params object[] args)
        {
            WriteColor(writer, Console.ForegroundColor, value, args);
        }

        private static void WriteLineColor(TextWriter writer, ConsoleColor color, string value, params object[] args)
        {
            WriteToTextWriter(writer, textWriter => textWriter.WriteLine(value, args), color);
        }

        private static void WriteColor(TextWriter writer, ConsoleColor color, string value, params object[] args)
        {
            WriteToTextWriter(writer, textWriter => textWriter.Write(value, args), color);
        }

        private static void WriteToTextWriter(TextWriter writer, Action<TextWriter> writeAction, ConsoleColor color)
        {
            var currentColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                writeAction(writer);
            }
            finally
            {
                Console.ForegroundColor = currentColor;
            }
        }
    }
}