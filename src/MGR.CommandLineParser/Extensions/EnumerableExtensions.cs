using System.IO;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerator<string> GetArgumentEnumerator([NotNull]this IEnumerable<string> arguments)
        {
            var enumerable = arguments as IList<string> ?? arguments.ToList();
            var firstArgument = enumerable.FirstOrDefault();
            if (string.IsNullOrEmpty(firstArgument))
            {
                return enumerable.GetEnumerator();
            }
            if (firstArgument.StartsWith("@", StringComparison.CurrentCulture))
            {
                if (!firstArgument.StartsWith("@@", StringComparison.CurrentCulture))
                {
                    var responseFileName = firstArgument.Remove(0, 1);
                    if (Path.GetExtension(responseFileName) == ".rsp" && File.Exists(responseFileName))
                    {
                        var responseFileContent = File.ReadAllLines(responseFileName);
                        return responseFileContent.AsEnumerable().GetEnumerator();
                    }
                }
                var firstArgumentWithoutArobase = firstArgument.Remove(0, 1);
                return new[] { firstArgumentWithoutArobase }.Concat(enumerable.Skip(1)).GetEnumerator();
            }
            return enumerable.GetEnumerator();
        }
    }
}
