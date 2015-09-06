using System;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines the converter type for a dictionary property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="ConverterAttribute"/> with the converter type.
        /// </summary>
        /// <param name="converterType"></param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = nameof(IConverter)), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = nameof(CommandLineParser))]
        public ConverterAttribute(Type converterType)
        {
            Guard.NotNull(converterType, nameof(converterType));

            if (!typeof (IConverter).IsAssignableFrom(converterType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ConverterAttributeTypeMustBeIConverter);
            }
            ConverterType = converterType;
        }

        /// <summary>
        /// Gets the type of the converter.
        /// </summary>
        public Type ConverterType { get; private set; }
    }
}