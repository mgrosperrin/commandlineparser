using System.IO;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Implementation of <see cref="IAssemblyFileProvider"/> for providing all files (*.dll and *.exe) in the current folder recursivly.
    /// </summary>
    public sealed class RecursiveAssemblyFileProvider : AssemblyFileProviderBase
    {
        /// <inheritdoc />
        protected override SearchOption SearchOption => SearchOption.AllDirectories;
    }
}