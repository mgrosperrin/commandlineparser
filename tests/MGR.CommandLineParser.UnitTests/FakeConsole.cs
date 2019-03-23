using System;
using System.Collections.Generic;
using System.Text;
using MGR.CommandLineParser.Extensibility;

namespace MGR.CommandLineParser.UnitTests
{
    public class FakeConsole : IConsole
    {
        private readonly List<Message> _messages = new List<Message>();
        private Message _currentMessage;
        public abstract class Message :IEquatable<Message>
        {
            private readonly StringBuilder _sb = new StringBuilder();

            internal void Add(string value)
            {
                _sb.Append(value);
            }

            public override string ToString() => _sb.ToString();

            public bool Equals(Message other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return GetType() == other.GetType() && Equals(_sb, other._sb);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((Message) obj);
            }

            public override int GetHashCode() => (_sb != null ? _sb.GetHashCode() : 0);
        }
        public class ErrorMessage : Message
        {

        }
        public class InformationMessage : Message
        {

        }
        public class WarningMessage : Message
        {

        }
        
        public void Write(string format, params object[] args)
        {
            if (!(_currentMessage is InformationMessage))
            {
                _currentMessage = new InformationMessage();
                _messages.Add(_currentMessage);
            }
            _currentMessage.Add(string.Format(format, args));
        }

        public void WriteError(string format, params object[] args)
        {
            if (!(_currentMessage is ErrorMessage))
            {
                _currentMessage = new ErrorMessage();
                _messages.Add(_currentMessage);
            }
            _currentMessage.Add(string.Format(format, args));
        }

        public void WriteWarning(string format, params object[] args)
        {
            if (!(_currentMessage is WarningMessage))
            {
                _currentMessage = new WarningMessage();
                _messages.Add(_currentMessage);
            }
            _currentMessage.Add(string.Format(format, args));
        }

        internal List<Message> Messages => _messages;
    }
}
