using System.IO;

namespace MGR.CommandLineParser
{
    public sealed class RecursiveAssemblyFileProvider : AssemblyFileProviderBase
    {
        protected override SearchOption SearchOption => SearchOption.AllDirectories;
    }
}