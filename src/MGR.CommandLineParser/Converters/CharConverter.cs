using System;
using System.Globalization;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    ///   Converter for the type <see cref="char" /> .
    /// </summary>
    public sealed class CharConverter : IConverter
    {
        /// <summary>
        ///   The target type of the converter ( <see cref="char" /> )..
        /// </summary>
        public Type TargetType
        {
            get { return typeof (Char); }
        }

        /// <summary>
        ///   Convert the <paramref name="value" /> to an instance of <see cref="Char" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> Not used. </param>
        /// <returns> The <see cref="Char" /> converted from the value. </returns>
        /// <exception cref="CommandLineParserException">Thrown if the
        ///   <paramref name="value" />
        ///   is not valid.</exception>
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return Char.Parse(value);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, CommonStrings.ExcConverterUnableConvertFormat, value, "Char"),
                                                     exception);
            }
        }
    }
}