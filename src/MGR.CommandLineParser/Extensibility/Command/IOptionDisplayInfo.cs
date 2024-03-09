namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// Represents the display information of an option.
/// </summary>
public interface IOptionDisplayInfo
{
    /// <summary>
    /// Gets the name of the option.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the alternates names of the option.
    /// </summary>
    IEnumerable<string> AlternateNames { get; }

    /// <summary>
    /// Gets the short name of the option.
    /// </summary>
    string ShortName { get; }

    /// <summary>
    /// Gets the description of the option.
    /// </summary>
    string Description { get; }
}