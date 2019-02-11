using System.Collections.Generic;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Defines a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>Return 0 is everything was right, an negative error code otherwise.</returns>
        int Execute();
        /// <summary>
        /// The list of arguments of the command.
        /// </summary>
        IList<string> Arguments { get; }
    }
}