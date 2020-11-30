using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Command.Lambda;
using MGR.CommandLineParser.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleApp
{
    internal static class ParserBuilderCalls
    {
        internal static async Task CreateAndCallDefaultParserBuilderAsync()
        {
            Console.WriteLine("ParserBuilderCalls.CreateAndCallDefaultParserBuilderAsync");
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
            Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

            var parserBuild = new ParserBuilder(new ParserOptions())
                .AddCommands(builder => builder.AddCommands<DeleteCommand>());
            var parser = parserBuild.BuildParser();
            var commandResult = await parser.Parse(arguments);
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
        internal static async Task CreateAndCallCustomizedParserBuilderAsync()
        {
            Console.WriteLine("ParserBuilderCalls.CreateAndCallCustomizedParserBuilderAsync");
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
            Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

            var serviceCollection = new ServiceCollection();
            var parserBuild = new ParserBuilder(new ParserOptions(), serviceCollection)
                .AddCommands(builder => builder.AddCommands<DeleteCommand>());
            var parser = parserBuild.BuildParser();
            var commandResult = await parser.Parse(arguments);
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
        internal static async Task CreateAndCallCustomizedWithCommandsParserBuilderAsync()
        {
            Console.WriteLine("ParserBuilderCalls.CreateAndCallCustomizedWithCommandsParserBuilderAsync");
            var arguments = new[] { "test", "--longName:3", "hello" };
            Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

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
                        return Task.FromResult(year + arg.Count());
                    });

            var parserBuild = new ParserBuilder(new ParserOptions(), serviceCollection)
                .AddCommands(builder => builder.AddCommands<DeleteCommand>());
            var parser = parserBuild.BuildParser();
            var commandResult = await parser.Parse(arguments);
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
