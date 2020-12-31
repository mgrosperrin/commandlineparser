using System;
using System.Threading.Tasks;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleApp
{
    internal static class ParserFactoryCalls
    {
        internal static async Task ConfigureParserAndGetParserFactoryAsync()
        {
            Console.WriteLine("ParserFactoryCalls.ConfigureParserAndGetParserFactoryAsync");
            // In Startup class
            void Configure(IServiceCollection services)
            {
                services.AddCommandLineParser(
                    options =>
                    {
                        options.Logo = "Parser via IParserFactory";
                        options.CommandLineName = "";
                    }
                ).AddCommands<DeleteCommand>();
            }

            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var scopedServiceProvider = serviceProvider.CreateScope().ServiceProvider;

            async Task ActionInController(IParserFactory factory)
            {
                var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
                Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

                var parser = factory.CreateParser();
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

            var parserFactory = scopedServiceProvider.GetRequiredService<IParserFactory>();
            await ActionInController(parserFactory);
        }

        internal static async Task ConfigureParserAndGetParserFactoryWithSpecificCommandAsync()
        {
            Console.WriteLine("ParserFactoryCalls.ConfigureParserAndGetParserFactoryWithSpecificCommandAsync");
            // In Startup class
            void Configure(IServiceCollection services)
            {
                services.AddCommandLineParser(
                    options =>
                    {
                        options.Logo = "Parser via IParserFactory";
                        options.CommandLineName = "";
                    }
                ).AddCommands<DeleteCommand>();
            }

            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var scopedServiceProvider = serviceProvider.CreateScope().ServiceProvider;

            async Task ActionInController(IParserFactory factory)
            {
                var arguments = new[] { @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
                Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

                var parser = factory.CreateParser();
                var commandResult = await parser.Parse<PackCommand>(arguments);
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

            var parserFactory = scopedServiceProvider.GetRequiredService<IParserFactory>();
            await ActionInController(parserFactory);
        }

        internal static async Task ConfigureParserAndGetParserFactoryWithDefaultCommandAsync()
        {
            Console.WriteLine("ParserFactoryCalls.ConfigureParserAndGetParserFactoryWithDefaultCommandAsync");
            // In Startup class
            void Configure(IServiceCollection services)
            {
                services.AddCommandLineParser(
                    options =>
                    {
                        options.Logo = "Parser via IParserFactory";
                        options.CommandLineName = "";
                    }
                ).AddCommands<DeleteCommand>();
            }

            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var scopedServiceProvider = serviceProvider.CreateScope().ServiceProvider;

            async Task ActionInController(IParserFactory factory)
            {
                var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "--properties", "Configuration=Release", "--build", "--symbols", "--msbuild-version", "14" };
                Console.WriteLine("Parse: '{0}'", string.Join(" ", arguments));

                var parser = factory.CreateParser();
                var commandResult = await parser.ParseWithDefaultCommand<DeleteCommand>(arguments);
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

            var parserFactory = scopedServiceProvider.GetRequiredService<IParserFactory>();
            await ActionInController(parserFactory);
        }
    }
}
