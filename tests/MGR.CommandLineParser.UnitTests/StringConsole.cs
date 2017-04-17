using System;
using System.Text;

namespace MGR.CommandLineParser.UnitTests
{
    internal class StringConsole : IConsole
    {
        private readonly StringBuilder _console = new StringBuilder();

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
            _console.AppendLine();
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
            var lastLineStartsAt = _console.ToString().LastIndexOf(Environment.NewLine);
            var indexInLastLine = _console.Length - lastLineStartsAt - 2;
            _console.Append(new string(' ', startIndex - indexInLastLine) + value);
        }

        public string AsString() => _console.ToString();
    }
}