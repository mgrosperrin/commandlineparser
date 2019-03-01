using System;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedComandOptionMetadata : CommandOptionMetadataBase
    {
        public LambdaBasedComandOptionMetadata(LambdaBasedOptionDisplayInfo optionDisplayInfo, string defaultValue, bool isRequired, Type optionType)
        : base(isRequired, GetMultiValueIndicator(optionType), optionDisplayInfo, defaultValue)
        { }
    }
}