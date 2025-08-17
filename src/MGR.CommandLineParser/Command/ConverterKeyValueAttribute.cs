using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the key and the value converter types for a dictionary property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
[Obsolete("Use the generic version of the MGR.CommandLineParser.Command.ConverterKeyValueAttribute if you use C#11+. This attribute will be removed in a future version.")]
public sealed class ConverterKeyValueAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of a <see cref="ConverterKeyValueAttribute"/> with the value converter type.
    /// </summary>
    /// <param name="valueConverterType">The type of the value converter.</param>
    /// <remarks>The key's converter is supposed to be the <see cref="StringConverter"/>.</remarks>
    public ConverterKeyValueAttribute(Type valueConverterType) : this(valueConverterType, typeof(StringConverter))
    {
    }
    /// <summary>
    /// Initializes a new instance of a <see cref="ConverterKeyValueAttribute"/> with the value and the key converter types.
    /// </summary>
    /// <param name="valueConverterType">The type of the value converter.</param>
    /// <param name="keyConverterType">The type of the key converter.</param>
    //[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IConverter"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CommandLineParser")]
    public ConverterKeyValueAttribute(Type valueConverterType, Type keyConverterType)
    {
        Guard.NotNull(keyConverterType, nameof(keyConverterType));
        Guard.NotNull(valueConverterType, nameof(valueConverterType));
        Guard.IsIConverter(keyConverterType, Constants.ExceptionMessages.ConverterKeyConverterTypeMustBeIConverter);
        Guard.IsIConverter(valueConverterType, Constants.ExceptionMessages.ConverterValueConverterTypeMustBeIConverter);

        KeyConverterType = keyConverterType;
        ValueConverterType = valueConverterType;
    }
    /// <summary>
    /// Gets the type of the key converter.
    /// </summary>
    public Type KeyConverterType { get; }
    /// <summary>
    /// Gets the type of the value converter.
    /// </summary>
    public Type ValueConverterType { get; }
}