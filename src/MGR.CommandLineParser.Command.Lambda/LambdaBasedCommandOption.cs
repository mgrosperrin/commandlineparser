using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command.Lambda;

[DebuggerDisplay("LambdaOption:{Metadata.DisplayInfo.Name}")]
internal class LambdaBasedCommandOption : ICommandOption
{
    private readonly IConverter _converter;

    internal LambdaBasedCommandOption(LambdaBasedCommandOptionMetadata commandOptionMetadata, Type optionType, IConverter converter,
        IEnumerable<ValidationAttribute> validationAttributes)
    {
        OptionType = optionType;
        _converter = converter;
        Metadata = commandOptionMetadata;
        ValidationAttributes = validationAttributes;
        var collectionType = Metadata.CollectionType;
        switch (collectionType)
        {
            case CommandOptionCollectionType.None:
                ValueAssigner = new LambdaBasedCommandOptionSimpleValueAssigner();
                break;
            case CommandOptionCollectionType.Collection:
                ValueAssigner = new LambdaBasedCommandOptionCollectionValueAssigner();
                break;
            case CommandOptionCollectionType.Dictionary:
                ValueAssigner = new LambdaBasedCommandOptionDictionaryValueAssigner();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(collectionType));
        }

        if (!string.IsNullOrEmpty(Metadata.DefaultValue))
        {
            AssignValue(Metadata.DefaultValue);
        }
    }

    internal Type OptionType { get; }
    internal ILambdaBasedCommandOptionValueAssigner ValueAssigner { get; }
    internal IEnumerable<ValidationAttribute> ValidationAttributes { get; }

    public bool ShouldProvideValue => OptionType != typeof(bool);
    public ICommandOptionMetadata Metadata { get; }

    public void AssignValue(string optionValue)
    {
        var value = _converter.Convert(optionValue, OptionType);
        ValueAssigner.AssignValue(value);
    }
}