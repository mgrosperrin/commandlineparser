using System;
using System.Globalization;

namespace MGR.CommandLineParser.Extensibility.Converters
{
    /// <summary>
    ///   Converter for the type <see cref="byte" /> .
    /// </summary>
    public sealed class ByteConverter : IConverter
    {
        /// <summary>
        ///   The target type of the converter ( <see cref="byte" /> )..
        /// </summary>
        public Type TargetType => typeof (byte);

        /// <summary>
        ///   Convert the <paramref name="value" /> to an instance of <see cref="byte" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> Not used. </param>
        /// <returns> The <see cref="byte" /> converted from the value. </returns>
        /// <exception cref="CommandLineParserException">Thrown if the
        ///   <paramref name="value" />
        ///   is not valid.</exception>
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return byte.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType),
                                                     exception);
            }
        }
    }
}