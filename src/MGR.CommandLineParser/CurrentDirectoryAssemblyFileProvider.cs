using System.IO;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Implementation of <see cref="IAssemblyFileProvider"/> for providing all files (*.dll and *.exe) in the current folder not recursivly (this is the current default implementation).
    /// </summary>
    public sealed class CurrentDirectoryAssemblyFileProvider : AssemblyFileProviderBase
    {
        /// <inheritdoc />
        protected override SearchOption SearchOption => SearchOption.TopDirectoryOnly;
    }
}