namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// Represents the metadata of a command.
/// </summary>
public interface ICommandMetadata
{
    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the command (if defined).
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the usage of the command (if defined).
    /// </summary>
    string Usage { get; }

    /// <summary>
    /// Gets the samples for the command.
    /// </summary>
    string[] Samples { get; }

    /// <summary>
    /// Determine if the command should be hidden from the help listing.
    /// </summary>
    bool HideFromHelpListing { get; }
}