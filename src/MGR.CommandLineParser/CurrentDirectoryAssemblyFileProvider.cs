using System.IO;

namespace MGR.CommandLineParser
{
    public sealed class CurrentDirectoryAssemblyFileProvider : AssemblyFileProviderBase
    {
        protected override SearchOption SearchOption => SearchOption.TopDirectoryOnly;
    }
}