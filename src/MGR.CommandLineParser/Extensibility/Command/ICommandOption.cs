
namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Represents a command's option.
    /// </summary>
    public interface ICommandOption
    {
        /// <summary>
        /// Defines if a value should be provided when assigned to the command's option
        /// </summary>
        /// <remarks>This is the case for the <see cref="bool"/> for example, where no value indicates a <code>true</code> value.</remarks>
        bool ShouldProvideValue { get; }

        /// <summary>
        /// Gets the metadata of the option.
        /// </summary>
        ICommandOptionMetadata Metadata { get; }

        /// <summary>
        /// Assigns a value to the command's option.
        /// </summary>
        /// <param name="optionValue"></param>
        void AssignValue(string optionValue);
    }
}