using System;
using MGR.CommandLineParser;

namespace SimpleApp
{
    internal class Program
    {
        private static int Main()
        {
            Console.ReadLine();
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Properties", "Configuration=Release", "-Build", "-Symbols", "-MSBuildVersion", "14" };
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            var commandResult = parser.Parse(arguments);
            Console.ReadLine();
            if (commandResult.IsValid)
            {
                return commandResult.Execute();
            }
            return (int)commandResult.ReturnCode;
        }
    }
}