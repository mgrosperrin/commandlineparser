using System;
using MGR.CommandLineParser.Extensibility.Converters;

// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser.Command
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extensions methods for the types <see cref="ConverterAttribute"/> and <see cref="ConverterKeyValueAttribute"/>.
    /// </summary>
    internal static class ConverterAttributeExtensions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="IConverter"/> specified by the <see cref="ConverterAttribute"/>.
        /// </summary>
        /// <param name="source">The <see cref="ConverterAttribute"/>.</param>
        /// <returns>A new instance of the <see cref="IConverter"/>.</returns>
        internal static IConverter BuildConverter(this ConverterAttribute source)
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.ConverterType) as IConverter;
        }
        /// <summary>
        /// Creates a new instance of the key <see cref="IConverter"/> specified by the <see cref="ConverterKeyValueAttribute"/>.
        /// </summary>
        /// <param name="source">The <see cref="ConverterKeyValueAttribute"/>.</param>
        /// <returns>A new instance of the <see cref="IConverter"/>.</returns>
        internal static IConverter BuildKeyConverter(this ConverterKeyValueAttribute source)
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.KeyConverterType) as IConverter;
        }
        /// <summary>
        /// Creates a new instance of the value <see cref="IConverter"/> specified by the <see cref="ConverterKeyValueAttribute"/>.
        /// </summary>
        /// <param name="source">The <see cref="ConverterKeyValueAttribute"/>.</param>
        /// <returns>A new instance of the <see cref="IConverter"/>.</returns>
        internal static IConverter BuildValueConverter(this ConverterKeyValueAttribute source)
        {
            Guard.NotNull(source, nameof(source));

            return Activator.CreateInstance(source.ValueConverterType) as IConverter;
        }
    }
}