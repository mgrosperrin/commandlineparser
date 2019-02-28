using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class LambdaBasedCollectionComandOptionMetadata: ICommandOptionMetadata
    {
        public LambdaBasedCollectionComandOptionMetadata(LambdaBasedOptionDisplayInfo optionDisplayInfo)
        {

        }
        public bool IsRequired { get; }
        public CommandOptionCollectionType CollectionType { get; }
        public IOptionDisplayInfo DisplayInfo { get; }
        public string DefaultValue { get; }
    }
}