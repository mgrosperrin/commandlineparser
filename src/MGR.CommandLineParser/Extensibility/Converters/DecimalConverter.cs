using System;
using System.Globalization;

namespace MGR.CommandLineParser.Extensibility.Converters
{
    /// <summary>
    ///   Converter for the type <see cref="decimal" /> .
    /// </summary>
    public sealed class DecimalConverter : IConverter
    {
        /// <summary>
        ///   The target type of the converter ( <see cref="decimal" /> )..
        /// </summary>
        public Type TargetType => typeof (decimal);

        /// <summary>
        ///   Convert the <paramref name="value" /> to an instance of <see cref="decimal" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> Not used. </param>
        /// <returns> The <see cref="decimal" /> converted from the value. </returns>
        /// <exception cref="CommandLineParserException">Thrown if the
        ///   <paramref name="value" />
        ///   is not valid.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Decimal.Parse(System.String,System.IFormatProvider)")]
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return decimal.Parse(value, CultureInfo.CurrentUICulture);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType),
                                                     exception);
            }
        }
    }
}