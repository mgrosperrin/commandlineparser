using System;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class LambdaBasedCommandOption: ICommandOption
    {
        public LambdaBasedCommandOption(LambdaBasedComandOptionMetadata commandOptionMetadata)
        {

        }
        public bool OptionalValue { get; }
        public ICommandOptionMetadata Metadata { get; }
        public void AssignValue(string optionValue) => throw new NotImplementedException();
    }
}