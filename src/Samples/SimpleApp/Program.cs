using MGR.CommandLineParser;

namespace SimpleApp
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            string[] arguments = new[] { "pack", @"MGR.CommandLineParser\MGR.CommandLineParser.csproj", "-Build", "-Properties", "Configuration=Release", "-Exclude", "Test", "-Symbols" };
            var commandResult = Parser.Create().Parse(arguments);
            if (commandResult.IsValid)
            {
                return commandResult.Execute();
            }
            return (int)commandResult.ReturnCode;
        }
    }
}