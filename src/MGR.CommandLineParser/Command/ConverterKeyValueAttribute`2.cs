using System;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command;

/// <summary>
/// Defines the key and the value converter types for a dictionary property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ConverterKeyValueAttribute<TKeyConverter, TValueConverter> : Attribute
    where TKeyConverter : IConverter
    where TValueConverter : IConverter
{ }
