using System.Collections.Generic;

namespace MGR.CommandLineParser
{
    public interface IAssemblyFileProvider
    {
        IEnumerable<string> GetFilesToLoad();
    }
}