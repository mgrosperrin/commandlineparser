using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///
    /// </summary>
    public class CommandValidationResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="validationErrors"></param>
        public CommandValidationResult(bool isValid, IEnumerable<ValidationResult> validationErrors)
        {
            IsValid = isValid;
            ValidationErrors = validationErrors;
        }
        /// <summary>
        ///
        /// </summary>
        public bool IsValid { get; }
        /// <summary>
        ///
        /// </summary>
        public IEnumerable<ValidationResult> ValidationErrors { get; }
    }
}
