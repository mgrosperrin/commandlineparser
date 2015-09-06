using System.Collections.Generic;
using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Defines the contract for the provider of assemblies to load, used by <see cref="DirectotyBrowsingCommandProvider"/>.
    /// </summary>
    public interface IAssemblyFileProvider
    {
        /// <summary>
        /// Returns the list of the files to load.
        /// </summary>
        /// <returns>A list of path.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [NotNull, ItemNotNull]
        IEnumerable<string> GetFilesToLoad();
    }
}