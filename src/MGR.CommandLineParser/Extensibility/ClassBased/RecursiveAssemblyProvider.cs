namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Implementation of <see cref="IAssemblyProvider"/> for providing all files (*.dll and *.exe) in the current folder recursively.
/// </summary>
public sealed class RecursiveAssemblyProvider : AssemblyProviderBase
{
    /// <summary>
    /// Gets the singleton instance of <see cref="RecursiveAssemblyProvider"/>.
    /// </summary>
    public static readonly IAssemblyProvider Instance = new RecursiveAssemblyProvider();
    private RecursiveAssemblyProvider() { }

    /// <inheritdoc />
    protected override SearchOption SearchOption => SearchOption.AllDirectories;
}