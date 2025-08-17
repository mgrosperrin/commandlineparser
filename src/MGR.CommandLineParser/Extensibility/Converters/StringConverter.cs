namespace MGR.CommandLineParser.Extensibility.Converters;

/// <summary>
/// Converter for the type <see cref="string"/>.
/// </summary>
public sealed class StringConverter : IConverter
{
    /// <summary>
    /// The target type of the converter (<see cref="string"/>).
    /// </summary>
    public Type TargetType => typeof(string);

    /// <summary>
    /// Convert the <paramref name="value"/> to an instance of <see cref="string"/>.
    /// </summary>
    /// <param name="value">The original value provided by the user.</param>
    /// <param name="concreteTargetType">Not used.</param>
    /// <returns>The <see cref="string"/> converted from the value.</returns>
    public object Convert(string value, Type concreteTargetType) => value;
}