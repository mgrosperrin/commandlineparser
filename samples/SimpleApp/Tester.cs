using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleApp
{
    internal static class Tester
    {
        public static async Task RunSampleTests()
        {
            //Console.ReadLine();
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
            var defaultPackArguments = new[] { @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
            var defaultDeleteArguments = new[] { "delete", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--no-prompt", "--source", "source1" };
            var helpArguments = new[] { "help" };
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandLineParser()
                .AddClassBasedCommands()
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
                            return Task.FromResult(year);
                        });
            serviceCollection.AddLogging(builder => builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddSeq()
                //.AddConsole(options => options.IncludeScopes = true)
                );
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            await ParseAndExecute(parser, serviceProvider, helpArguments);

            await ParseAndExecute(parser, serviceProvider, helpArguments);

            await ParseAndExecute(parser, serviceProvider, new[] { "test", "--longName:3", "hello" });

            await ParseWithDefaultAndExecute<PackCommand>(parser, serviceProvider, arguments);
            await ParseWithDefaultAndExecute<PackCommand>(parser, serviceProvider, defaultPackArguments);
            await ParseWithDefaultAndExecute<PackCommand>(parser, serviceProvider, defaultDeleteArguments);

            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        static async Task ParseWithDefaultAndExecute<TCommand>(IParser parser, ServiceProvider serviceProvider, string[] arguments)
            where TCommand : class, ICommand
        {
            await ParseFuncAndExecute(parser, serviceProvider, arguments,
                (p, args, scope) => p.ParseWithDefaultCommand<TCommand>(args, scope));
        }
        static async Task ParseAndExecute(IParser parser, ServiceProvider serviceProvider, string[] arguments)
        {
            await ParseFuncAndExecute(parser, serviceProvider, arguments,
                (p, args, scope) => p.Parse(args, scope));
        }
        static async Task ParseFuncAndExecute(IParser parser, ServiceProvider serviceProvider, string[] arguments, Func<IParser, IEnumerable<string>, IServiceProvider, ParsingResult> parseFunc)
        {
            Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));
            var scopedServiceProvider = serviceProvider.CreateScope().ServiceProvider;
            var commandResult = parseFunc(parser, arguments, scopedServiceProvider);
            if (commandResult.IsValid)
            {
                var executionResult = await commandResult.CommandObject.ExecuteAsync();
                Console.WriteLine("Execution result: {0}", executionResult);
            }
            else
            {
                Console.WriteLine("Invalid parsing");
            }
        }
    }
}
