namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Represents the metadata of a command's option.
    /// </summary>
    public interface ICommandOptionMetadata
    {
        /// <summary>
        ///     Defines if an option is required or not.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        ///     Defines the type of collection of the option.
        /// </summary>
        CommandOptionCollectionType CollectionType { get; }

        /// <summary>
        ///     Gets the display information of the option.
        /// </summary>
        OptionDisplayInfo DisplayInfo { get; }

        /// <summary>
        ///     Gets the default value of the option, if explicitly defined.
        /// </summary>
        string DefaultValue { get; }
    }
}