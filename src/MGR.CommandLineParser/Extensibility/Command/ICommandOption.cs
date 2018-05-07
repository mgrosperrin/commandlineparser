using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Represents a command's option.
    /// </summary>
    public interface ICommandOption
    {
        /// <summary>
        /// Defines if the value is optional when assigned to the command's option
        /// </summary>
        /// <remarks>This is the case for the <see cref="bool"/> for example, where no value indicates a <code>true</code> value.</remarks>
        bool OptionalValue { get; }
        /// <summary>
        /// Assigns a value to the command's option.
        /// </summary>
        /// <param name="optionValue"></param>
        /// <param name="command"></param>
        void AssignValue(string optionValue, ICommand command);
    }
}