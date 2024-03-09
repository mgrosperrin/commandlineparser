namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// Represents an <see cref="ICommandOption"/> that combines a collection of <see cref="ICommandOption"/>.
/// </summary>
/// <remarks>This class can be used to combined options together (for example boolean options).</remarks>
public class CombinedCommandOption : ICommandOption
{
    private readonly string _commandName;
    private readonly IEnumerable<ICommandOption> _commandOptions;
    private readonly string _optionName;
    /// <summary>
    /// Create an instance of <see cref="CombinedCommandOption"/>.
    /// </summary>
    /// <param name="optionName">The name of the option used in the command line.</param>
    /// <param name="commandName">The name of the command being parsed.</param>
    /// <param name="commandOptions">The <see cref="ICommandOption"/> that are combined together.</param>
    public CombinedCommandOption(string optionName, string commandName, IEnumerable<ICommandOption> commandOptions)
    {
        _commandOptions = commandOptions;
        _optionName = optionName;
        _commandName = commandName;
    }

    /// <inheritdoc />
    public bool ShouldProvideValue => _commandOptions.Aggregate(false, (optionalValue, commandOption) => optionalValue || commandOption.ShouldProvideValue);

    /// <inheritdoc />
    public void AssignValue(string optionValue)
    {
        if (ShouldProvideValue && optionValue == null)
        {
            throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionValueRequired(_commandName, _optionName));
        }

        foreach (var commandOption in _commandOptions)
        {
            commandOption.AssignValue(optionValue);
        }
    }

    /// <summary>
    /// Always throws an <see cref="InvalidOperationException"/> because the <see cref="ICommandOptionMetadata"/> cannot be combined.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public ICommandOptionMetadata Metadata
    {
        get { throw new InvalidOperationException(); }
    }
}