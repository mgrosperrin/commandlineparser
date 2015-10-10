using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Defines the contract for the provider of assemblies to load, used by <see cref="AssemblyBrowsingCommandTypeProvider"/>.
    /// </summary>
    public interface IAssemblyProvider
    {
        /// <summary>
        /// Returns the list of the files to load.
        /// </summary>
        /// <returns>A list of path.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [NotNull, ItemNotNull]
        IEnumerable<Assembly> GetAssembliesToBrowse();
    }
}