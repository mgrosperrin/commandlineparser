using System;
using System.IO;
using System.Text;

namespace MGR.CommandLineParser.UnitTests
{
    public class StringConsole : IConsole
    {
        private readonly StringBuilder _consoleOutBuilder = new StringBuilder();
        private readonly StringBuilder _consoleErrorBuilder = new StringBuilder();
        private readonly DefaultConsole _console;

        public StringConsole()
        {
            _console = new DefaultConsole(new StringWriter(_consoleOutBuilder), new StringWriter(_consoleErrorBuilder));
        }

        public void Write(string format, params object[] args)
        {
            _console.Write(format, args);
        }

        public void WriteLine()
        {
            _console.WriteLine();
        }

        public void WriteLine(string value)
        {
            _console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            _console.WriteLine(format, args);
        }

        public void WriteError(string value)
        {
            _console.WriteError(value);
        }

        public void WriteError(string format, params object[] args)
        {
            _console.WriteError(format, args);
        }

        public void WriteWarning(string value)
        {
            _console.WriteWarning(value);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _console.WriteWarning(format, args);
        }

        public string OutAsString() => _consoleOutBuilder.ToString();
        public string ErrorAsString() => _consoleErrorBuilder.ToString();
    }
}