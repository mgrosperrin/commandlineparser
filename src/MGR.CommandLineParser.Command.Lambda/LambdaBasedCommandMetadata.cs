using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandMetadata : ICommandMetadata
    {
        internal LambdaBasedCommandMetadata(string commandName, string description, string usage, string[] samples, bool hideFromHelpListing)
        {
            Name = commandName;
            Description = description;
            Usage = usage;
            Samples = samples;
            HideFromHelpListing = hideFromHelpListing;
        }

        public string Name { get; }
        public string Description { get; }
        public string Usage { get; }
        public string[] Samples { get; }
        public bool HideFromHelpListing { get; }
    }
}