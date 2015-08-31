using System;
using System.Globalization;
using System.IO;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser
{
    internal sealed class DefaultConsole : IConsole
    {
        private static int CursorLeft
        {
            get
            {
                try
                {
                    return Console.CursorLeft;
                }
                catch (IOException)
                {
                    return 0;
                }
            }
        }

        private static int WindowWidth
        {
            get
            {
                try
                {
                    return Console.WindowWidth;
                }
                catch (IOException)
                {
                    return 60;
                }
            }
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void WriteError(string value)
        {
            WriteError(value, null);
        }

        public void WriteError(string format, params object[] args)
        {
            WriteColor(Console.Error, ConsoleColor.Red, format, args);
        }

        public void WriteWarning(string value)
        {
            WriteWarning(value, new object[0]);
        }

        public void WriteWarning(string value, params object[] args)
        {
            var message = string.Format(CultureInfo.CurrentCulture, Strings.Console_WarningFormat, value);
            WriteColor(Console.Out, ConsoleColor.Yellow, message, args);
        }

        private static void WriteColor(TextWriter writer, ConsoleColor color, string value, params object[] args)
        {
            var currentColor = Console.ForegroundColor;
            try
            {
                currentColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                writer.WriteLine(value, args);
            }
            finally
            {
                Console.ForegroundColor = currentColor;
            }
        }

        public void PrintJustified(int startIndex, string value)
        {
            PrintJustified(startIndex, value, WindowWidth);
        }

        internal static void PrintJustified(int startIndex, string value, int maxWidth)
        {
            if (maxWidth > startIndex)
            {
                maxWidth = maxWidth - startIndex - 1;
            }

            while (value.Length > 0)
            {
                // Trim whitespace at the beginning
                value = value.TrimStart();
                // Calculate the number of chars to print based on the width of the System.Console
                var length = Math.Min(value.Length, maxWidth);
                // Text we can print without overflowing the System.Console.
                var content = value.Substring(0, length);
                var leftPadding = startIndex + length - CursorLeft;
                // Print it with the correct padding
                Console.WriteLine(content.PadLeft(leftPadding));
                // Get the next substring to be printed
                value = value.Substring(content.Length);
            }
        }
    }
}