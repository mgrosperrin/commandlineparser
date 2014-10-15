using System;
using System.Globalization;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    /// Converter for the type <see cref="long"/>.
    /// </summary>
    public sealed class Int64Converter : IConverter
    {
        /// <summary>
        /// The target type of the converter (<see cref="long"/>)..
        /// </summary>
        public Type TargetType
        {
            get { return typeof (Int64); }
        }

        /// <summary>
        /// Convert the <paramref name="value"/> to an instance of <see cref="Int64"/>.
        /// </summary>
        /// <param name="value">The original value provided by the user.</param>
        /// <param name="concreteTargetType">Not used.</param>
        /// <returns>The <see cref="Int64"/> converted from the value.</returns>
        /// <exception cref="CommandLineParserException">Thrown if the <paramref name="value"/> is not valid.</exception>
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return Int64.Parse(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, CommonStrings.ExcConverterUnableConvertFormat, value, "Int64"),
                                                     exception);
            }
        }
    }
}