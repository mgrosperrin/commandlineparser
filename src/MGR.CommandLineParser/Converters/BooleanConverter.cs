using System;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    ///     Converter for the type <see cref="bool" /> .
    /// </summary>
    public sealed class BooleanConverter : IConverter
    {
        /// <summary>
        ///     The target type of the converter ( <see cref="bool" /> )..
        /// </summary>
        public Type TargetType => typeof (bool);


        /// <summary>
        ///     Convert the <paramref name="value" /> to an instance of <see cref="bool" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> Not used. </param>
        /// <returns> The <see cref="bool" /> converted from the value. </returns>
        /// <remarks>
        ///     The value can be '-', 'False' or 'false' to specify false, '+', 'True' or 'true' to specify true.
        /// </remarks>
        /// <exception cref="CommandLineParserException">
        ///     Thrown if the
        ///     <paramref name="value" />
        ///     is not valid.
        /// </exception>
        public object Convert(string value, Type concreteTargetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            if (value.Equals("-", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (value.Equals("+", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            try
            {
                return bool.Parse(value);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType), exception);
            }
        }
    }
}