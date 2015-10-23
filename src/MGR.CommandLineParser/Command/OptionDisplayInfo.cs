namespace MGR.CommandLineParser.Command
{
    /// <summary>
    /// Represents the display information of an option.
    /// </summary>
    public sealed class OptionDisplayInfo
    {
        /// <summary>
        /// Gets the name of the option.
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// Gets the shortname of the option.
        /// </summary>
        public string ShortName { get; internal set; }
        /// <summary>
        /// Gets the description of the option.
        /// </summary>
        public string Description { get; internal set; }
    }
}