using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the converter type for a property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ConverterAttribute<TConverter> : Attribute
    where TConverter : IConverter
{ }