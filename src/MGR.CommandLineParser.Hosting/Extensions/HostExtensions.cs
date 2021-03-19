using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MGR.CommandLineParser.Hosting.Extensions
{
    /// <summary>
    /// Extension's methods for <see cref="IHost"/>.
    /// </summary>
    public static class HostExtensions
    {
        /// <summary>
        /// Parse the command line and execute the command if it is valid.
        /// </summary>
        /// <param name="host">The configured <see cref="IHost"/>.</param>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="cancellationToken">The token to trigger shutdown.</param>
        /// <returns>A code that represents the result of the parsing and the execution of the command.</returns>
        public static async Task<int> ParseCommandLineAndExecuteAsync(this IHost host, string[] args, CancellationToken cancellationToken = default) => await ParseCommandLineAndExecuteAsync(host, args, (parser, arguments) => parser.Parse(arguments), cancellationToken);

        /// <summary>
        /// Parse the command line for a specific command and execute the command if it is valid. The name of the command should not be in the arguments list.
        /// </summary>
        /// <param name="host">The configured <see cref="IHost"/>.</param>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="cancellationToken">The token to trigger shutdown.</param>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <remarks>This method can only be used with class-based command.</remarks>
        /// <returns>A code that represents the result of the parsing and the execution of the command.</returns>
        public static async Task<int> ParseCommandLineAndExecuteAsync<TCommand>(this IHost host, string[] args, CancellationToken cancellationToken = default) where TCommand : class, ICommand => await ParseCommandLineAndExecuteAsync(host, args, (parser, arguments) => parser.Parse<TCommand>(arguments), cancellationToken);

        /// <summary>
        /// Parse the command line and execute the command if it is valid. If the name of the command is not the first argument, fallback to the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the default command.</typeparam>
        /// <param name="host">The configured <see cref="IHost"/>.</param>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="cancellationToken">The token to trigger shutdown.</param>
        /// <remarks>This method can only be used with class-based command.</remarks>
        /// <returns>A code that represents the result of the parsing and the execution of the command.</returns>
        public static async Task<int> ParseCommandLineWithDefaultCommandAndExecuteAsync<TCommand>(this IHost host, string[] args, CancellationToken cancellationToken = default) where TCommand : class, ICommand => await ParseCommandLineAndExecuteAsync(host, args, (parser, arguments) => parser.ParseWithDefaultCommand<TCommand>(arguments), cancellationToken);

        private static async Task<int> ParseCommandLineAndExecuteAsync(IHost host, string[] args, Func<IParser, IEnumerable<string>, Task<ParsingResult>> parseArguments, CancellationToken cancellationToken)
        {
            var parserContext = host.Services.GetRequiredService<ParserContext>();
            parserContext.ParseArguments = parseArguments;
            parserContext.Arguments = args;
            await host.RunAsync(cancellationToken);
            var parsingAndExecutionResult = parserContext.ParsingAndExecutionResult;
            return parsingAndExecutionResult;
        }
    }
}
