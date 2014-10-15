using System.Collections.Generic;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines a command with specific usage samples.
    /// </summary>
    public interface ISampleCommand : ICommand
    {
        /// <summary>
        /// Gets the usage samples.
        /// </summary>
        IEnumerable<string> Samples { get; }
    }
}