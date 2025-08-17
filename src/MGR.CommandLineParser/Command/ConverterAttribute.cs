using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the converter type for a property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
[Obsolete("Use the generic version of the MGR.CommandLineParser.Command.ConverterAttribute if you use C#11+. This attribute will be removed in a future version.")]
public sealed class ConverterAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of a <see cref="ConverterAttribute"/> with the converter type.
    /// </summary>
    /// <param name="converterType"></param>
    public ConverterAttribute(Type converterType)
    {
        Guard.NotNull(converterType, nameof(converterType));

        if (!converterType.IsType<IConverter>())
        {
            throw new CommandLineParserException(Constants.ExceptionMessages.ConverterAttributeTypeMustBeIConverter);
        }
        ConverterType = converterType;
    }

    /// <summary>
    /// Gets the type of the converter.
    /// </summary>
    public Type ConverterType { get; }
}