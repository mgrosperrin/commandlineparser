using System;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Tests.Commands;

namespace SimpleApp
{
    internal class Program
    {
        private static int Main()
        {
            Console.ReadLine();
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var defaultPackArguments = new[] { @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var defaultDeleteArguments = new[] { "delete", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-NoPrompt", "-Source", "source1" };
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            var commandResult = parser.Parse(arguments);
            Console.WriteLine(commandResult.Command);
            var defaultCommandResult = parser.ParseWithDefaultCommand<PackCommand>(arguments);
            Console.WriteLine(defaultCommandResult.Command);
            var defaultPackCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultPackArguments);
            Console.WriteLine(defaultPackCommandResult.Command);
            var defaultDeleteCommandResult = parser.ParseWithDefaultCommand<PackCommand>(defaultDeleteArguments);
            Console.WriteLine(defaultDeleteCommandResult.Command);
            Console.ReadLine();
            if (commandResult.IsValid)
            {
                return commandResult.Execute();
            }
            return (int)commandResult.ReturnCode;
        }
    }
}