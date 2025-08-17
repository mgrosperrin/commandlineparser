using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Defines the contract for the provider of assemblies to load, used by <see cref="AssemblyBrowsingClassBasedCommandTypeProvider"/>.
/// </summary>
public interface IAssemblyProvider
{
    /// <summary>
    /// Returns the list of the files to load.
    /// </summary>
    /// <returns>A list of path.</returns>
    IEnumerable<Assembly> GetAssembliesToBrowse();
}