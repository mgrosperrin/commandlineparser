using System.Globalization;

namespace MGR.CommandLineParser.Extensibility.Converters;

/// <summary>
/// Converter for the type <see cref="short" />.
/// </summary>
public sealed class Int16Converter : IConverter
{
    /// <summary>
    /// The target type of the converter ( <see cref="short" /> ).
    /// </summary>
    public Type TargetType => typeof(short);

    /// <summary>
    /// Convert the <paramref name="value" /> to an instance of <see cref="short" />.
    /// </summary>
    /// <param name="value"> The original value provided by the user. </param>
    /// <param name="concreteTargetType"> Not used. </param>
    /// <returns> The <see cref="short" /> converted from the value. </returns>
    /// <exception cref="CommandLineParserException">Thrown if the
    /// <paramref name="value" />
    /// is not valid.</exception>
    public object Convert(string value, Type concreteTargetType)
    {
        try
        {
            return short.Parse(value, CultureInfo.CurrentUICulture);
        }
        catch (FormatException exception)
        {
            throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType), exception);
        }
    }
}