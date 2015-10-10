using System;
using MGR.CommandLineParser;

namespace SimpleApp
{
    internal class Program
    {
        private static int Main()
        {
            var arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Build", "-Properties", "Configuration=Release", "-Exclude", "Test", "-Symbols" };
            Console.WriteLine("Press enter");
            Console.ReadLine();
            var parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            Console.WriteLine("Press enter");
            Console.ReadLine();
            var commandResult = parser.Parse(arguments);
            Console.WriteLine("Press enter");
            Console.ReadLine();
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            commandResult = parser.Parse(arguments);
            Console.WriteLine("Press enter");
            Console.ReadLine();
            if (commandResult.IsValid)
            {
                return commandResult.Execute();
            }
            return (int)commandResult.ReturnCode;
        }
    }
}