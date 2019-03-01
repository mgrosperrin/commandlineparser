using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedOptionDisplayInfo : IOptionDisplayInfo
    {
        public LambdaBasedOptionDisplayInfo(string name, IEnumerable<string> alternateNames, string shortName, string description)
        {
            Name = name;
            AlternateNames = alternateNames ?? Enumerable.Empty<string>();
            ShortName = shortName;
            Description = description ?? string.Empty;
        }

        public string Name { get; }
        public IEnumerable<string> AlternateNames { get; }
        public string ShortName { get; }
        public string Description { get; }
    }
}