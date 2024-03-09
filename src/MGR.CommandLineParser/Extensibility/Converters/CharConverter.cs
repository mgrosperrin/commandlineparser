namespace MGR.CommandLineParser.Extensibility.Converters;

/// <summary>
/// Converter for the type <see cref="char" />.
/// </summary>
public sealed class CharConverter : IConverter
{
    /// <summary>
    /// The target type of the converter ( <see cref="char" /> ).
    /// </summary>
    public Type TargetType => typeof(char);

    /// <summary>
    /// Convert the <paramref name="value" /> to an instance of <see cref="char" />.
    /// </summary>
    /// <param name="value"> The original value provided by the user. </param>
    /// <param name="concreteTargetType"> Not used. </param>
    /// <returns> The <see cref="char" /> converted from the value. </returns>
    /// <exception cref="CommandLineParserException">Thrown if the
    /// <paramref name="value" />
    /// is not valid.</exception>
    public object Convert(string value, Type concreteTargetType)
    {
        try
        {
            return char.Parse(value);
        }
        catch (FormatException exception)
        {
            throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType),
                                                 exception);
        }
    }
}