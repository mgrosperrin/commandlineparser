using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MGR.CommandLineParser.Hosting.Extensions
{
    /// <summary>
    /// Extension's methods for <see cref="HostBuilder"/>.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the <see cref="HostBuilder"/> to run the parser.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="HostBuilder"/> to configure.</param>
        /// <param name="configureParser">An action to configure the <see cref="IParser"/>.</param>
        /// <returns>The configured <see cref="HostBuilder"/>.</returns>
        public static HostBuilder ConfigureParser(this HostBuilder hostBuilder, Action<CommandLineParserBuilder> configureParser)
        {
            hostBuilder.ConfigureParser(configureParser, _ => { });
            return hostBuilder;
        }
        /// <summary>
        /// Configures the <see cref="HostBuilder"/> to run the parser.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="HostBuilder"/> to configure.</param>
        /// <param name="configureParser">An action to configure the <see cref="IParser"/>.</param>
        /// <param name="configureParserOptions">An action to configure the <see cref="IParserOptions"/> of the parser.</param>
        /// <returns>The configured <see cref="HostBuilder"/>.</returns>
        public static HostBuilder ConfigureParser(this HostBuilder hostBuilder, Action<CommandLineParserBuilder> configureParser, Action<ParserOptionsBuilder> configureParserOptions)
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
        /// <param name="hostBuilder">The configured <see cref="HostBuilder"/>.</param>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="cancellationToken">The token to trigger shutdown.</param>
        /// <returns>A code that represents the result of the parsing and the execution of the command.</returns>
        public static async Task<int> ParseCommandLineAndExecuteAsync(this HostBuilder hostBuilder, string[] args, CancellationToken cancellationToken = default)
        {
            var host = hostBuilder.Build();
            var parserContext = host.Services.GetRequiredService<ParserContext>();
            parserContext.Arguments = args;
            await host.RunAsync(cancellationToken);
            var parsingResult = parserContext.ParsingResult;
            if (parsingResult.IsValid)
            {
                return await parsingResult.ExecuteAsync();
            }
            return (int)parserContext.ParsingResult.ParsingResultCode;
        }
    }
}
