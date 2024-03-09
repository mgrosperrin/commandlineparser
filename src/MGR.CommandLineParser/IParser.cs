using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser;

/// <summary>
/// Represents a parser.
/// </summary>
public interface IParser
{
    /// <summary>
    /// Gets the name of the current tools.
    /// </summary>
    string CommandLineName { get; }

    /// <summary>
    /// Gets the current logo (name + version) of the current tools.
    /// </summary>
    string Logo { get; }

    /// <summary>
    /// Parse the supplied arguments for a specific command. The name of the command should not be in the arguments list.
    /// </summary>
    /// <typeparam name="TCommandHandler">The type of the command.</typeparam>
    /// <typeparam name="TCommandData">The type of the command.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <remarks>This method can only be used with class-based command.</remarks>
    /// <returns>The result of the parsing of the arguments.</returns>
    Task<ParsingResult> Parse<TCommandHandler, TCommandData>(IEnumerable<string> arguments) where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new();

    /// <summary>
    /// Parse the supplied arguments.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The result of the parsing of the arguments.</returns>
    Task<ParsingResult> Parse(IEnumerable<string> arguments);

    /// <summary>
    /// Parse the supplied arguments. If the name of the command is not the first argument, fallback to the specified command. The default command can only be class-based.
    /// </summary>
    /// <typeparam name="TCommandHandler">The type of the command.</typeparam>
    /// <typeparam name="TCommandData">The type of the default command.</typeparam>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The result of the parsing of the arguments.</returns>
    Task<ParsingResult> ParseWithDefaultCommand<TCommandHandler, TCommandData>(IEnumerable<string> arguments)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new();
}