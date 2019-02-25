using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents the result of the parsing.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command (defined to a specific type if you call the generic Parse method, <see cref="ICommand"/> otherwise).</typeparam>
    //[SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public class CommandResult<TCommand> : ParsingResult
        where TCommand : class, ICommand
    {
        private readonly TCommand _command;

        internal CommandResult(TCommand command, CommandParsingResultCode parsingResultCode)
            : this(command, parsingResultCode, new List<ValidationResult>())
        { }
        internal CommandResult(TCommand command, CommandParsingResultCode parsingResultCode, List<ValidationResult> validationResults)
        :base(validationResults, parsingResultCode)
        {
            _command = command;
        }
        /// <summary>
        /// The resulting command.
        /// </summary>
        public TCommand Command => _command;

        /// <summary>
        /// Defines if the command is in a valid state (parsing/validating the options).
        /// </summary>
        public override bool IsValid => ParsingResultCode == CommandParsingResultCode.Success && !ValidationResults.Any();

        /// <summary>
        /// Executes the underlying command.
        /// </summary>
        /// <returns>Returns the result of the Execute method of the command.</returns>
        /// <exception cref="CommandLineParserException">Thrown if the underlying command is null, or if the command is in an invalid state.</exception>
        public override Task<int> ExecuteAsync()
        {
            if (_command == null || !IsValid)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.NoValidCommand);
            }
            return Command.ExecuteAsync();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is CommandResult<TCommand>)
            {
                var commandResult = (CommandResult<TCommand>)obj;
                return commandResult.Command == Command;
            }
            return false;
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (_command == null)
            {
                return ParsingResultCode.GetHashCode();
            }
            return _command.GetHashCode();
        }

        ///// <inheritdoc />
        //public static bool operator ==(CommandResult<TCommand> first, CommandResult<TCommand> second) => first.Equals(second);
        ///// <inheritdoc />
        //public static bool operator !=(CommandResult<TCommand> first, CommandResult<TCommand> second) => !first.Equals(second);
    }
}