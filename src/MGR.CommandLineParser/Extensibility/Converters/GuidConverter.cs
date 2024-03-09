namespace MGR.CommandLineParser.Extensibility.Converters;

/// <summary>
/// Converter for the type <see cref="Guid" />.
/// </summary>
public sealed class GuidConverter : IConverter
{
    /// <summary>
    /// The target type of the converter ( <see cref="Guid" /> ).
    /// </summary>
    public Type TargetType => typeof(Guid);

    /// <summary>
    /// Convert the <paramref name="value" /> to an instance of <see cref="Guid" />.
    /// </summary>
    /// <param name="value"> The original value provided by the user. </param>
    /// <param name="concreteTargetType"> Not used. </param>
    /// <returns> The <see cref="Guid" /> converted from the value. </returns>
    /// <exception cref="CommandLineParserException">Thrown if the
    /// <paramref name="value" />
    /// is not valid.</exception>
    public object Convert(string value, Type concreteTargetType)
    {
        try
        {
            return Guid.Parse(value);
        }
        catch (FormatException exception)
        {
            throw new CommandLineParserException(Constants.ExceptionMessages.FormatConverterUnableConvert(value, TargetType), exception);
        }
    }
}