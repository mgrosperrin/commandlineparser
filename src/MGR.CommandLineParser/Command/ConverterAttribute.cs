using System;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines the converter type for a dictionary property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="ConverterAttribute"/> with the converter type.
        /// </summary>
        /// <param name="converterType"></param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IConverter"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CommandLineParser")]
        public ConverterAttribute(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException("converterType");
            }
            if (!typeof (IConverter).IsAssignableFrom(converterType))
            {
                throw new CommandLineParserException("The converter type must implement MGR.CommandLineParser.Command.IConverter");
            }
            ConverterType = converterType;
        }

        /// <summary>
        /// Gets the type of the converter.
        /// </summary>
        public Type ConverterType { get; private set; }
    }
}