using System;
using System.Runtime.Serialization;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Exception thrown by the parser if a technical errors occurs.
    /// </summary>
    [Serializable]
    public class CommandLineParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MGR.CommandLineParser.CommandLineParserException"/> class.
        /// </summary>
        public CommandLineParserException()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MGR.CommandLineParser.CommandLineParserException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CommandLineParserException(string message)
            : base(message)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MGR.CommandLineParser.CommandLineParserException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="System.Exception"/>  that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CommandLineParserException(string message, Exception innerException)
            : base(message, innerException)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MGR.CommandLineParser.CommandLineParserException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
        protected CommandLineParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}