using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    /// Represents an interface that allow accessing to the raw <see cref="ICommand"/> instance.
    /// </summary>
    public interface IClassBasedCommandObject
    {
        /// <summary>
        /// Gets the raw <see cref="ICommand"/> instance for the <see cref="ICommandObject"/>.
        /// </summary>
        ICommand Command { get; }
    }
}