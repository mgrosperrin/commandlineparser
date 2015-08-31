using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents the result of the parsing.
    /// </summary>
    /// <typeparam name="T">The type of the command (defined to a specific type if you call the generic Parse method, <see cref="ICommand"/> otherwise).</typeparam>
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct CommandResult<T> where T : class, ICommand
    {
        private readonly T _command;
        private readonly CommandResultCode _returnCode;
        private readonly List<ValidationResult> _validationResults;

        internal CommandResult(T command, CommandResultCode returnCode)
            : this(command, returnCode, new List<ValidationResult>())
        { }
        internal CommandResult(T command, CommandResultCode returnCode, List<ValidationResult> validationResults)
        {
            _command = command;
            _returnCode = returnCode;
            _validationResults = validationResults;
        }
        /// <summary>
        /// The resulting command.
        /// </summary>
        public T Command => _command;

        /// <summary>
        /// Defines if the command is in a valid state (parsing/validating the options).
        /// </summary>
        public bool IsValid => _returnCode == CommandResultCode.Ok;

        /// <summary>
        /// The return code of the parsing.
        /// </summary>
        public CommandResultCode ReturnCode => _returnCode;

        /// <summary>
        /// The validation results. If there was no validation errors, the enumeration is empty.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults => _validationResults.AsEnumerable();

        /// <summary>
        /// Executes the underlying command.
        /// </summary>
        /// <returns>Returns the result of the Execute method of the command.</returns>
        /// <exception cref="CommandLineParserException">Thrown if the underlying command is null, or if the command is in an invalid state.</exception>
        public int Execute()
        {
            if (_command == null || !IsValid)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.NoValidCommand);
            }
            return Command.Execute();
        }
    }
}