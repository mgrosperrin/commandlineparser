using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandOptionMetadata : ICommandOptionMetadata
    {
        public CommandOptionMetadata(OptionDisplayInfo displayInfo, bool isRequired, string defaultValue)
        {
            DisplayInfo = displayInfo;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
        }
        public bool IsRequired { get; }

        public CommandOptionCollectionType CollectionType => CommandOptionCollectionType.None;

        public IOptionDisplayInfo DisplayInfo { get; }

        public string DefaultValue { get; }
    }
}