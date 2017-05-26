using System;

namespace MGR.CommandLineParser.Extensibility.Converters
{
    /// <summary>
    /// Define a converter
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// The target type of the converter.
        /// </summary>
        Type TargetType { get; }
        /// <summary>
        /// Convert the <paramref name="value"/> to an instance of <paramref name="concreteTargetType"/>.
        /// </summary>
        /// <param name="value">The original value provided by the user.</param>
        /// <param name="concreteTargetType">The concrete type expected by the option.</param>
        /// <returns>The converted value.</returns>
        object Convert(string value, Type concreteTargetType);
    }
}