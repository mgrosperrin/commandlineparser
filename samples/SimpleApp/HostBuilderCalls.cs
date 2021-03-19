using System;
using System.Threading;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleApp
{
    internal static class HostBuilderCalls
    {
        internal static async Task CreateAndCallDefaultParserBuilderAsync()
        {
            var arguments = new[] { "run", "--opt", "14" };
            Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureParser(builder =>
            {
                builder.AddCommand("run",
                    commandBuilder =>
                    {
                        commandBuilder.AddOption<int>("opt", "o",
                            o => o.Required());
                    },
                    context =>
                    {
                        var opt = context.GetOptionValue<int>("opt");
                        return Task.FromResult(opt);
                    });
            });
            var parsingResult = await hostBuilder.ParseCommandLineAndExecuteAsync(arguments, CancellationToken.None);
            Console.WriteLine("Execution result: {0}", parsingResult);
        }
    }
}
