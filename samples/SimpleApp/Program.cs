using System;
using System.Threading;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleApp
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            //Console.ReadLine();
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var defaultPackArguments = new[] { @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var defaultDeleteArguments = new[] { "delete", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-NoPrompt", "-Source", "source1" };
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandLineParser()
                .AddLogging(builder => builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddSeq()
                    .AddConsole(options => options.IncludeScopes = true));
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser(serviceProvider);
            //var commandResult = parser.Parse(arguments);
            //Console.WriteLine(commandResult.Command);

            //var defaultCommandResult = parser.ParseWithDefaultCommand<PackCommand>(arguments);
            //Console.WriteLine(defaultCommandResult.Command);
            var defaultPackCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultPackArguments);
            Console.WriteLine(defaultPackCommandResult.Command);
            //var defaultDeleteCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultDeleteArguments);
            //Console.WriteLine(defaultDeleteCommandResult.Command);
            //Console.ReadLine();
            //if (commandResult.IsValid)
            {
              //  return commandResult.Execute();
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
            return 0;//(int)commandResult.ReturnCode;
        }
    }
}