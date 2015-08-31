using System;
using System.Globalization;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    ///   Converter for the type <see cref="Enum" /> .
    /// </summary>
    public sealed class EnumConverter : IConverter
    {
        /// <summary>
        ///   The target type of the converter ( <see cref="Enum" /> )..
        /// </summary>
        public Type TargetType => typeof(Enum);

        /// <summary>
        ///   Convert the <paramref name="value" /> to an instance of <see cref="Enum" /> .
        /// </summary>
        /// <param name="value"> The original value provided by the user. </param>
        /// <param name="concreteTargetType"> The concrete enum type. </param>
        /// <returns> The <see cref="Enum" /> converted from the value. </returns>
        /// <exception cref="CommandLineParserException">Thrown if the
        ///   <paramref name="value" />
        ///   is not valid or if
        ///   <paramref name="concreteTargetType" />
        ///   is not an enum.</exception>
        public object Convert(string value, Type concreteTargetType)
        {
            if (concreteTargetType == null)
            {
                throw new ArgumentNullException("concreteTargetType");
            }
            if (!TargetType.IsAssignableFrom(concreteTargetType))
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, "The specified concrete target type ({0}) is not an enum type.",
                                                                   concreteTargetType.Name));
            }
            try
            {
                var enumValue = Enum.Parse(concreteTargetType, value, true);
                if (!(Enum.IsDefined(concreteTargetType, enumValue) | enumValue.ToString().Contains(",")))
                {
                    throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, "The specified value '{0}' is not correct the type '{1}'.",
                                                                       value, concreteTargetType.Name));
                }
                return enumValue;
            }
            catch (ArgumentException exception)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, concreteTargetType), exception);
            }
        }
    }
}