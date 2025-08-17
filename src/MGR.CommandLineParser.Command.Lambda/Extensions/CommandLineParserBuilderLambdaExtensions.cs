using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Extensibility.Command;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="CommandLineParserBuilder"/>.
/// </summary>
public static class CommandLineParserBuilderLambdaExtensions
{
    /// <summary>
    /// Add a lambda-based command.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineParserBuilder"/> to configure.</param>
    /// <param name="commandName">The name of the command.</param>
    /// <param name="defineCommand">An action to define the command.</param>
    /// <param name="executeCommand">A function that represent the execution of the command.</param>
    /// <returns>The <see cref="CommandLineParserBuilder" /> so that additional calls can be chained.</returns>
    public static CommandLineParserBuilder AddCommand(this CommandLineParserBuilder builder,
        string commandName,
        Action<CommandBuilder> defineCommand,
        Func<CommandExecutionContext, CancellationToken, Task<int>> executeCommand)
    {
        var commandBuilder = new CommandBuilder(commandName, executeCommand);
        (defineCommand ?? (_ => { }))(commandBuilder);

        builder.Services.AddScoped<ICommandTypeProvider>(serviceProvider => new LambdaBasedCommandTypeProvider(commandBuilder.BuildCommandType(serviceProvider)));

        return builder;
    }
}
