using System;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace MGR.CommandLineParser.Command.Lambda
{
    /// <summary>
    /// Extension methods for <see cref="CommandLineParserBuilder"/>.
    /// </summary>
    public static class CommandLineParserBuilderExtensions
    {
        /// <summary>
        /// Add a lambda-based command.
        /// </summary>
        /// <param name="builder">The <see cref="CommandLineParserBuilder"/>.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="defineCommand">An action to define the command.</param>
        /// <param name="executeCommand">A function that represent the execution of the command.</param>
        /// <returns>The <see cref="CommandLineParserBuilder"/>.</returns>
        public static CommandLineParserBuilder AddCommand(this CommandLineParserBuilder builder,
            string commandName,
            Action<CommandBuilder> defineCommand,
            Func<CommandExecutionContext, Task<int>> executeCommand)
        {
            var commandBuilder = new CommandBuilder(commandName, executeCommand);
            (defineCommand ?? (_ => {}))(commandBuilder);

            builder.Services.AddScoped<ICommandTypeProvider>(serviceProvider => new LambdaBasedCommandTypeProvider(commandBuilder.BuildCommandType(serviceProvider)));

            return builder;
        }
    }
}
