using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents the result of the parsing operation.
    /// </summary>
    public sealed class ParsingResult
    {
        internal ParsingResult(ICommandObject commandObject, IEnumerable<ValidationResult> validationResults, CommandParsingResultCode parsingResultCode)
        {
            CommandObject = commandObject;
            ValidationResults = validationResults ?? Enumerable.Empty<ValidationResult>();
            ParsingResultCode = parsingResultCode;
        }

        /// <summary>
        /// Gets the validation results. If there was no validation errors, the enumeration is empty.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults { get; }

        /// <summary>
        /// Defines if the command is in a valid state (parsing and validating the options).
        /// </summary>
        public bool IsValid => ParsingResultCode == CommandParsingResultCode.Success && !ValidationResults.Any();

        /// <summary>
        /// The return code of the parsing operation.
        /// </summary>
        public CommandParsingResultCode ParsingResultCode { get; }

        /// <summary>
        /// Gets the raw command object;
        /// </summary>
        public ICommandObject CommandObject { get; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <exception cref="CommandLineParserException">Thrown if the <seealso cref="CommandObject"/> is null or <seealso cref="IsValid"/> is <code>false</code>.</exception>
        /// <returns>A task that represents the execution of the command.</returns>
        public Task<int> ExecuteAsync()
        {
            if (CommandObject == null || !IsValid)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.NoValidCommand);
            }
            return CommandObject.ExecuteAsync();
        }
    }
}
