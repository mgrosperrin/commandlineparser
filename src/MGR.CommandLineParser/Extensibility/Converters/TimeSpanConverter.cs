﻿using System;
using System.Globalization;

namespace MGR.CommandLineParser.Extensibility.Converters
{
    /// <summary>
    /// Converter for the type <see cref="TimeSpan"/>.
    /// </summary>
    public sealed class TimeSpanConverter : IConverter
    {
        /// <summary>
        /// The target type of the converter (<see cref="TimeSpan"/>).
        /// </summary>
        public Type TargetType => typeof (TimeSpan);

        /// <summary>
        /// Convert the <paramref name="value"/> to an instance of <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">The original value provided by the user.</param>
        /// <param name="concreteTargetType">Not used.</param>
        /// <returns>The <see cref="TimeSpan"/> converted from the value.</returns>
        /// <exception cref="CommandLineParserException">Thrown if the <paramref name="value"/> is not valid.</exception>
        //[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TimeSpan")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.TimeSpan.Parse(System.String,System.IFormatProvider)")]
        public object Convert(string value, Type concreteTargetType)
        {
            try
            {
                return TimeSpan.Parse(value, CultureInfo.CurrentUICulture);
            }
            catch (FormatException exception)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType), exception);
            }
        }
    }
}