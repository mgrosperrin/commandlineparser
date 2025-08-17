using System.ComponentModel.DataAnnotations;

namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// Represents the result of the validation of a command.
/// </summary>
public class CommandValidationResult
{
    /// <summary>
    /// Creates a new instance of <see cref="CommandValidationResult"/>.
    /// </summary>
    /// <param name="isValid">Specify if the validation pass or not.</param>
    /// <param name="validationErrors">The list of validation errors.</param>
    public CommandValidationResult(bool isValid, IEnumerable<ValidationResult> validationErrors)
    {
        IsValid = isValid;
        ValidationErrors = validationErrors;
    }
    /// <summary>
    /// Gets <code>true</code> if the validation pass, <code>false</code> elsewhere.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public IEnumerable<ValidationResult> ValidationErrors { get; }
}
