namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// Encapsulates a command
/// </summary>
public interface ICommandObjectBuilder
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="argument"></param>
    void AddArguments(string argument);
    /// <summary>
    /// Find an option based on its name.
    /// </summary>
    /// <param name="optionName">The name (short or long form) of the option.</param>
    /// <returns>The <see cref="ICommandOption"/> representing the option of the command.</returns>
    ICommandOption? FindOption(string optionName);
    /// <summary>
    /// Find an option based on its short name.
    /// </summary>
    /// <param name="optionShortName">The short name of the option.</param>
    /// <returns>The <see cref="ICommandOption"/> representing the option of the command.</returns>
    ICommandOption? FindOptionByShortName(string optionShortName);

    /// <summary>
    /// Generate the command object representing the command being parsed.
    /// </summary>
    /// <returns>An object representing the parsed command.</returns>
    ICommandObject GenerateCommandObject();

    /// <summary>
    /// Validates the command after having been parsed, and before being used.
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/>.</param>
    /// <returns>The result of the validation of the command.</returns>
    CommandValidationResult Validate(IServiceProvider serviceProvider);
}
