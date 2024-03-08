using System;
using MGR.CommandLineParser.Extensibility.Converters;

// ReSharper disable once CheckNamespace
namespace MGR.CommandLineParser.Command
{
    internal static class ConverterAttributeExtensions
    {
#pragma warning disable CS0618 // Type or member is obsolete
        internal static IConverter BuildConverter(this ConverterAttribute source)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.ConverterType) as IConverter;
        }
#pragma warning disable CS0618 // Type or member is obsolete
        internal static IConverter BuildKeyConverter(this ConverterKeyValueAttribute source)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.KeyConverterType) as IConverter;
        }
#pragma warning disable CS0618 // Type or member is obsolete
        internal static IConverter BuildValueConverter(this ConverterKeyValueAttribute source)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.ValueConverterType) as IConverter;
        }
    }
}