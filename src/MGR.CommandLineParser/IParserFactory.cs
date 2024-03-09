
namespace MGR.CommandLineParser;

/// <summary>
/// A factory abstractions to create a <see cref="IParser"/>.
/// </summary>
public interface IParserFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="IParser"/>.
    /// </summary>
    /// <returns>A new <see cref="IParser"/>.</returns>
    IParser CreateParser();
}
