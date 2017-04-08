using System;
using System.Text;

namespace MGR.CommandLineParser.IntegrationTests
{
    internal class MockedConsole : IConsole
    {
        [ThreadStatic]
        private static MockedConsole _currentConsole;
        public static MockedConsole CurrentConsole => _currentConsole = _currentConsole ?? new MockedConsole();

        private readonly StringBuilder _console = new StringBuilder();

        public void Reset()
        {
            _console.Clear();
        }

        public void Write(string format, params object[] args)
        {
            _console.AppendFormat(format, args);
        }

        public void WriteLine()
        {
            _console.AppendLine();
        }

        public void WriteLine(string value)
        {
            _console.AppendLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            _console.AppendFormat(format, args);
        }

        public void WriteError(string value)
        {
            _console.AppendLine(value);
        }

        public void WriteError(string format, params object[] args)
        {
            _console.AppendFormat(format, args);
        }

        public void WriteWarning(string value)
        {
            _console.AppendLine(value);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _console.AppendFormat(format, args);
        }

        public void PrintJustified(int startIndex, string value)
        {
            _console.AppendLine(new string(' ', startIndex) + value);
        }
    }
}