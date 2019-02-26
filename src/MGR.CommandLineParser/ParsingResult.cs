using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ParsingResult
    {
        private readonly ICommandObject _commandObject;

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandObject"></param>
        /// <param name="validationResults"></param>
        /// <param name="parsingResultCode"></param>
        public ParsingResult(ICommandObject commandObject, IEnumerable<ValidationResult> validationResults, CommandParsingResultCode parsingResultCode)
        {
            _commandObject = commandObject;
            ValidationResults = validationResults ?? Enumerable.Empty<ValidationResult>();
            ParsingResultCode = parsingResultCode;
        }
        /// <summary>
        /// The validation results. If there was no validation errors, the enumeration is empty.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults { get; }

        /// <summary>
        /// Defines if the command is in a valid state (parsing/validating the options).
        /// </summary>
        public bool IsValid => ParsingResultCode == CommandParsingResultCode.Success && !ValidationResults.Any();

        /// <summary>
        /// The return code of the parsing.
        /// </summary>
        public CommandParsingResultCode ParsingResultCode { get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Task<int> ExecuteAsync()
        {
            if (_commandObject == null || !IsValid)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.NoValidCommand);
            }
            return _commandObject.ExecuteAsync();
        }
    }
}
