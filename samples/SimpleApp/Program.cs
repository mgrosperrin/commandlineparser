using System.Threading;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleApp
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {

            await Tester.RunSampleTests();

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
            var parsingResult =  await hostBuilder.ParseCommandLineAndExecuteAsync(args, CancellationToken.None);
            return parsingResult;
        }
    }
}