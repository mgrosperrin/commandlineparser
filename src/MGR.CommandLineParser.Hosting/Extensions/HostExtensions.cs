using System.Threading;
using System.Threading.Tasks;
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
        public static async Task<int> ParseCommandLineAndExecuteAsync(this IHost host, string[] args, CancellationToken cancellationToken = default)
        {
            var parserContext = host.Services.GetRequiredService<ParserContext>();
            parserContext.Arguments = args;
            await host.RunAsync(cancellationToken);
            var parsingAndExecutionResult = parserContext.ParsingAndExecutionResult;
            return parsingAndExecutionResult;
        }
    }
}
