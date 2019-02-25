using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ParsingResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="validationResults"></param>
        /// <param name="parsingResultCode"></param>
        protected ParsingResult(IEnumerable<ValidationResult> validationResults, CommandParsingResultCode parsingResultCode)
        {
            ValidationResults = validationResults;
            ParsingResultCode = parsingResultCode;
        }
        /// <summary>
        /// The validation results. If there was no validation errors, the enumeration is empty.
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults { get; }

        /// <summary>
        /// Defines if the command is in a valid state (parsing/validating the options).
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// The return code of the parsing.
        /// </summary>
        public CommandParsingResultCode ParsingResultCode { get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract Task<int> ExecuteAsync();
    }
}
