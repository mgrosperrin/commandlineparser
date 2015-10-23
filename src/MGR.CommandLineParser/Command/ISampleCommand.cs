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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<string> GetSamples();
    }
}