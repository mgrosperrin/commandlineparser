using System;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines the key and the value converter types for a dictionary property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConverterKeyValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="ConverterKeyValueAttribute"/> with the value converter type.
        /// </summary>
        /// <param name="valueConverterType">The type of the value converter.</param>
        /// <remarks>The key's converter is supposed to be the <see cref="StringConverter"/>.</remarks>
        public ConverterKeyValueAttribute(Type valueConverterType) : this(valueConverterType, typeof (StringConverter))
        {
        }
        /// <summary>
        /// Initializes a new instance of a <see cref="ConverterKeyValueAttribute"/> with the value and the key converter types.
        /// </summary>
        /// <param name="valueConverterType">The type of the value converter.</param>
        /// <param name="keyConverterType">The type of the key converter.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IConverter"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CommandLineParser")]
        public ConverterKeyValueAttribute(Type valueConverterType, Type keyConverterType)
        {
            if (keyConverterType == null)
            {
                throw new ArgumentNullException(nameof(keyConverterType));
            }
            if (!typeof (IConverter).IsAssignableFrom(keyConverterType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ConverterKeyConverterTypeMustBeIConverter);
            }
            KeyConverterType = keyConverterType;
            if (valueConverterType == null)
            {
                throw new ArgumentNullException(nameof(valueConverterType));
            }
            if (!typeof (IConverter).IsAssignableFrom(valueConverterType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ConverterValueConverterTypeMustBeIConverter);
            }
            ValueConverterType = valueConverterType;
        }
        /// <summary>
        /// Gets the type of the key converter.
        /// </summary>
        public Type KeyConverterType { get; private set; }
        /// <summary>
        /// Gets the type of the value converter.
        /// </summary>
        public Type ValueConverterType { get; private set; }
    }
}