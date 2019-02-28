using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class LambdaBasedOptionDisplayInfo: IOptionDisplayInfo
    {
        public string Name { get; }
        public IEnumerable<string> AlternateNames { get; }
        public string ShortName { get; }
        public string Description { get; }
    }
}