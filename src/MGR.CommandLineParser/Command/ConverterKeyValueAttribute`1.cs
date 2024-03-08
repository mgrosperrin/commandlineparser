using System;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the converter type for a dictionary property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ConverterKeyValueAttribute<TKeyConverter> : ConverterKeyValueAttribute<TKeyConverter, StringConverter>
    where TKeyConverter : IConverter
{ }
