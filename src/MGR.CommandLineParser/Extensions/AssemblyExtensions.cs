using MGR.CommandLineParser;

namespace System.Reflection;

internal static class AssemblyExtensions
{
    internal static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> source, Func<Type, bool> predicate)
    {
        Guard.NotNull(predicate, nameof(predicate));

        var result = new List<Type>();
        if (source == null)
        {
            return result;
        }

        foreach (var assembly in source.Where(assembly => assembly != null && !assembly.IsDynamic))
        {
            Type[] exportedTypes;

            try
            {
                exportedTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                exportedTypes = ex.Types;
            }

            catch
            {
                // We deliberately ignore all other exceptions.
                continue;
            }

            if (exportedTypes != null)
            {
                result.AddRange(exportedTypes.Where(predicate));
            }
        }
        return result;
    }
}