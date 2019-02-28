using System;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace MGR.CommandLineParser.Command.Lambda
{
    public static class CommandLineParserBuilderExtensions
    {
        public static CommandLineParserBuilder AddCommand(this CommandLineParserBuilder builder,
            string commandName,
            Action<CommandBuilder> buildCommand,
            Func<CommandContext, Task<int>> executeCommand)
        {
            var commandBuilder = new CommandBuilder(commandName, executeCommand);
            (buildCommand ?? (_ => {}))(commandBuilder);

            builder.Services.AddSingleton<ICommandTypeProvider>(new LambdaBasedCommandTypeProvider(commandBuilder.BuildCommandType()));

            return builder;
        }
    }
}
