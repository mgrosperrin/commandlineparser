using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda;

internal class LambdaBasedCommandOptionMetadata : CommandOptionMetadataBase
{
    public LambdaBasedCommandOptionMetadata(LambdaBasedOptionDisplayInfo optionDisplayInfo, string defaultValue, bool isRequired, Type optionType)
    : base(isRequired, GetMultiValueIndicator(optionType), optionDisplayInfo, defaultValue)
    { }
}