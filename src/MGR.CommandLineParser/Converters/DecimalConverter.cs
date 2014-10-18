using System;
using System.Globalization;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    ///   Converter for the type <see cref="decimal" /> .
    /// </summary>
    public sealed class DecimalConverter : IConverter
    {
        /// <summary>
        ///   The target type of the converter ( <see cref="decimal" /> )..
        /// </summary>
        public Type TargetType
        {
            get { return typeof (Decimal); }
        }

        /// <summary>
        ///   Convert the <paramref name="value" /> to an instance of <see cref="Decimal" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> Not used. </param>
        /// <returns> The <see cref="Decimal" /> converted from the value. </returns>
        /// <exception cref="CommandLineParserException">Thrown if the
        ///   <paramref name="value" />
        ///   is not valid.</exception>
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return Decimal.Parse(value, CultureInfo.CurrentUICulture);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, CommonStrings.ExcConverterUnableConvertFormat, value, "Decimal"),
                                                     exception);
            }
        }
    }
}