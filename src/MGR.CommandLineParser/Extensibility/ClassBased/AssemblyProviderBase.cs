using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

/// <summary>
/// Base class for providing all files (*.dll and *.exe) in the current folder (recursive or not).
/// </summary>
public abstract class AssemblyProviderBase : IAssemblyProvider
{
    /// <summary>
    /// Gets the recursively options for browsing the current folder.
    /// </summary>
    protected abstract SearchOption SearchOption { get; }

    private IEnumerable<string> GetFilesToLoad()
    {
        var thisDirectory = Environment.CurrentDirectory;
        foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.exe", SearchOption))
        {
            yield return new FileInfo(item).FullName;
        }
        foreach (var item in Directory.EnumerateFiles(thisDirectory, "*.dll", SearchOption))
        {
            yield return new FileInfo(item).FullName;
        }
    }

    /// <inheritdoc />
    public IEnumerable<Assembly> GetAssembliesToBrowse()
    {
        if (DependencyContext.Default == null)
        {
            return [];
        }
        var alreadyLoadedLibraries = DependencyContext.Default.RuntimeLibraries;
        foreach (var assemblyFile in GetFilesToLoad())
        {
            var assemblyNameFromFile = Path.GetFileNameWithoutExtension(assemblyFile);
            if (alreadyLoadedLibraries.Any(runtimeLibrary => runtimeLibrary.Name.Equals(assemblyNameFromFile, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            try
            {
                Assembly.LoadFile(assemblyFile);
            }
            catch
            {
                // ignored
            }
        }

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies;
    }
}