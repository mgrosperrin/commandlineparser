namespace MGR.CommandLineParser.Extensibility.Command;

/// <summary>
/// The different types of collection that an command's option can be.
/// </summary>
public enum CommandOptionCollectionType
{
    /// <summary>
    /// Not a collection.
    /// </summary>
    None = 0,

    /// <summary>
    /// A simple collection.
    /// </summary>
    Collection = 1,

    /// <summary>
    /// A dictionary.
    /// </summary>
    Dictionary = 2
}