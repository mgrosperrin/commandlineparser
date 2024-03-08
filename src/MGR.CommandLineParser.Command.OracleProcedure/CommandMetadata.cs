using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandMetadata : ICommandMetadata
    {
        public CommandMetadata(string name)
        {
            Name = name;
            Description = name;
            Samples = new string[0];
        }
        public string Name { get; }

        public string Description { get; }

        public string Usage => string.Empty;

        public string[] Samples { get; }

        public bool HideFromHelpListing => false;
    }
}
