using System.Collections.Generic;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Define a command provider.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface ICommandProvider
    {
        /// <summary>
        /// Returns all commands instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<ICommand> GetAllCommands();
    }
}