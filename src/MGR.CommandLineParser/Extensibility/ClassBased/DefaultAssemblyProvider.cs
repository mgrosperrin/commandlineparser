using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal sealed class DefaultAssemblyProvider : IAssemblyProvider
{
    private readonly Assembly _assembly;

    internal DefaultAssemblyProvider(Assembly assembly)
    {
        _assembly = assembly;
    }

    public IEnumerable<Assembly> GetAssembliesToBrowse()
    {
        yield return _assembly;
    }
}