using System;
using System.Globalization;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    ///   Converter for the type <see cref="byte" /> .
    /// </summary>
    public sealed class ByteConverter : IConverter
    {
        /// <summary>
        ///   The target type of the converter ( <see cref="byte" /> )..
        /// </summary>
        public Type TargetType
        {
            get { return typeof (Byte); }
        }

        /// <summary>
        ///   Convert the <paramref name="value" /> to an instance of <see cref="Byte" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> Not used. </param>
        /// <returns> The <see cref="Byte" /> converted from the value. </returns>
        /// <exception cref="CommandLineParserException">Thrown if the
        ///   <paramref name="value" />
        ///   is not valid.</exception>
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return Byte.Parse(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, CommonStrings.ExcConverterUnableConvertFormat, value, "Byte"),
                                                     exception);
            }
        }
    }
}