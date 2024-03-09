namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Implementation of <see cref="IAssemblyProvider"/> for providing all files (*.dll and *.exe) in the current folder NOT recursively (this is the current default implementation).
/// </summary>
public sealed class CurrentDirectoryAssemblyProvider : AssemblyProviderBase
{
    /// <summary>
    /// Gets the singleton instance of <see cref="CurrentDirectoryAssemblyProvider"/>.
    /// </summary>
    public static readonly IAssemblyProvider Instance = new CurrentDirectoryAssemblyProvider();
    /// <summary>
    /// Create a new <see cref="CurrentDirectoryAssemblyProvider"/>.
    /// </summary>

    /// <inheritdoc />
    protected override SearchOption SearchOption => SearchOption.TopDirectoryOnly;
}