namespace MGR.CommandLineParser.Command.Lambda;

/// <summary>
/// Represents the context of execution of a lambda-based command.
/// </summary>
public class CommandExecutionContext
{
    private readonly IEnumerable<LambdaBasedCommandOption> _commandOptions;

    internal CommandExecutionContext(IEnumerable<LambdaBasedCommandOption> commandOptions, List<string> arguments, IServiceProvider serviceProvider)
    {
        _commandOptions = commandOptions;
        Arguments = arguments;
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets the arguments of the command.
    /// </summary>
    public IEnumerable<string> Arguments { get; }

    /// <summary>
    /// Gets the value of an option.
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <param name="name">The name of the option.</param>
    /// <returns>The value of the option.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If no options are found.</exception>
    /// <exception cref="InvalidOperationException">If the type of the option do not match the specified type.</exception>
    public T? GetOptionValue<T>(string name)
    {
        var option = _commandOptions.FirstOrDefault(o => o.Metadata.DisplayInfo.Name == name);
        if (option == null)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        if (!typeof(T).IsAssignableFrom(option.OptionType))
        {
            throw new InvalidOperationException($"The type of the option ({option.OptionType}) do not match the specified type ({typeof(T)})");
        }

        var rawValue = option.ValueAssigner.GetValue();
        if (rawValue == null)
        {
            return default;
        }

        return (T)rawValue;
    }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }
}