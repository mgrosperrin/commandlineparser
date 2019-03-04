using System;
using System.ComponentModel.DataAnnotations;
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
                                    optionBuilder => {
                                        optionBuilder.Required()
                         .AddValidation(new RangeAttribute(2, 7));
                                    });
                        },
                        context =>
                        {
                            var year = context.GetOptionValue<int>("longName");
                            var arg = context.Arguments;
                            return Task.FromResult(0);
                        });
            serviceCollection.AddLogging(builder => builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddSeq()
                //.AddConsole(options => options.IncludeScopes = true)
                );
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            var commandResult = parser.Parse(arguments, serviceProvider);
            var lambdaCommand = parser.Parse(new[] { "test", "--longName:3", "hello" }, serviceProvider);

            var defaultCommandResult = parser.ParseWithDefaultCommand<PackCommand>(arguments, serviceProvider);

            var defaultPackCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultPackArguments, serviceProvider);

            var defaultDeleteCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultDeleteArguments, serviceProvider);

            //Console.ReadLine();
            Thread.Sleep(TimeSpan.FromSeconds(10));
            if (lambdaCommand.IsValid)
            {
                return await lambdaCommand.CommandObject.ExecuteAsync();
            }
            return (int)lambdaCommand.ParsingResultCode;
        }
    }
}