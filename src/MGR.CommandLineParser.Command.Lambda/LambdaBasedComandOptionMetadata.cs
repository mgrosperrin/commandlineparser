using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class LambdaBasedComandOptionMetadata: ICommandOptionMetadata
    {
        public LambdaBasedComandOptionMetadata(LambdaBasedOptionDisplayInfo optionDisplayInfo)
        {

        }
        public bool IsRequired { get; }
        public CommandOptionCollectionType CollectionType { get; }
        public IOptionDisplayInfo DisplayInfo { get; }
        public string DefaultValue { get; }
    }
}