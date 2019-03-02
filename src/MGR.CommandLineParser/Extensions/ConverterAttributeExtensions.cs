using System;
using MGR.CommandLineParser.Extensibility.Converters;

// ReSharper disable once CheckNamespace
namespace MGR.CommandLineParser.Command
{
    internal static class ConverterAttributeExtensions
    {
        internal static IConverter BuildConverter(this ConverterAttribute source)
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.ConverterType) as IConverter;
        }
        internal static IConverter BuildKeyConverter(this ConverterKeyValueAttribute source)
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.KeyConverterType) as IConverter;
        }
        internal static IConverter BuildValueConverter(this ConverterKeyValueAttribute source)
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.ValueConverterType) as IConverter;
        }
    }
}