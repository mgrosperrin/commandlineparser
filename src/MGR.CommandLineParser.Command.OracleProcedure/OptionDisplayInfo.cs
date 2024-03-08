using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class OptionDisplayInfo : IOptionDisplayInfo
    {
        public OptionDisplayInfo(string name)
        {
            Name = name;
            Description = name;
        }
        public string Name { get; }

        public IEnumerable<string> AlternateNames => Enumerable.Empty<string>();

        public string ShortName => string.Empty;

        public string Description { get; }
    }
}
