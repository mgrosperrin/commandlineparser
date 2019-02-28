using System;
using System.Threading;
using System.Threading.Tasks;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleApp
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            //Console.ReadLine();
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var defaultPackArguments = new[] { @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var defaultDeleteArguments = new[] { "delete", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-NoPrompt", "-Source", "source1" };
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandLineParser()
                .AddCommand(
                        "test",
                        commandBuilder =>
                        {
                            commandBuilder
                                .WithDescription("Description of command")
                                .AddOption<int>(
                                    "longName",
                                    "shortName",
                                    optionBuilder => { optionBuilder.Required(); });
                        },
                        context =>
                        {
                            var year = context.GetOption<int>("year");
                            var arg = context.Arguments;
                            throw new NotImplementedException();
                        });
            serviceCollection.AddLogging(builder => builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddSeq()
                    .AddConsole(options => options.IncludeScopes = true));
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser(serviceProvider);
            var commandResult = parser.Parse(arguments);

            var defaultCommandResult = parser.ParseWithDefaultCommand<PackCommand>(arguments);

            var defaultPackCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultPackArguments);

            var defaultDeleteCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultDeleteArguments);

            //Console.ReadLine();
            Thread.Sleep(TimeSpan.FromSeconds(10));
            if (commandResult.IsValid)
            {
                return await commandResult.CommandObject.ExecuteAsync();
            }
            return (int)commandResult.ParsingResultCode;
        }
    }
}