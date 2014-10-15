using System;

namespace MGR.CommandLineParser.Converters
{
    /// <summary>
    /// Converter for the type <see cref="Uri"/>.
    /// </summary>
    public sealed class UriConverter : IConverter
    {
        /// <summary>
        /// The target type of the converter (<see cref="Uri"/>)..
        /// </summary>
        public Type TargetType
        {
            get { return typeof (Uri); }
        }

        /// <summary>
        /// Convert the <paramref name="value"/> to an instance of <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The original value provided by the user.</param>
        /// <param name="concreteTargetType">Not used.</param>
        /// <returns>The <see cref="Uri"/> converted from the value.</returns>
        public object Convert(string value, Type concreteTargetType)
        {
            return new Uri(value, UriKind.RelativeOrAbsolute);
        }
    }
}