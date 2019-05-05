using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MGR.CommandLineParser.Hosting.Extensions
{
    /// <summary>
    /// Extension's methods for <see cref="IHostBuilder"/>.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the <see cref="IHostBuilder"/> to run the parser.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to configure.</param>
        /// <param name="configureParser">An action to configure the <see cref="IParser"/>.</param>
        /// <returns>The configured <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureParser(this IHostBuilder hostBuilder, Action<CommandLineParserBuilder> configureParser)
        {
            hostBuilder.ConfigureParser(configureParser, _ => { });
            return hostBuilder;
        }
        /// <summary>
        /// Configures the <see cref="IHostBuilder"/> to run the parser.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to configure.</param>
        /// <param name="configureParser">An action to configure the <see cref="IParser"/>.</param>
        /// <param name="configureParserOptions">An action to configure the <see cref="ParserOptions"/> of the parser.</param>
        /// <returns>The configured <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureParser(this IHostBuilder hostBuilder, Action<CommandLineParserBuilder> configureParser, Action<ParserOptions> configureParserOptions)
        {
            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddScoped<ParserContext>();
                    services.AddHostedService<ParserHostedService>();
                    var builder = services.AddCommandLineParser(configureParserOptions);
                    configureParser(builder);
                }
            );
            return hostBuilder;
        }

        /// <summary>
        /// Parse the command line and execute the command if it is valid.
        /// </summary>
        /// <param name="hostBuilder">The configured <see cref="IHostBuilder"/>.</param>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="cancellationToken">The token to trigger shutdown.</param>
        /// <returns>A code that represents the result of the parsing and the execution of the command.</returns>
        public static async Task<int> ParseCommandLineAndExecuteAsync(this IHostBuilder hostBuilder, string[] args, CancellationToken cancellationToken = default)
        {
            var host = hostBuilder.Build();
            var executionResult = await host.ParseCommandLineAndExecuteAsync(args, cancellationToken);
            return executionResult;
        }
    }
}
