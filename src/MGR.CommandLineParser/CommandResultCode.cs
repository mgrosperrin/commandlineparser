namespace MGR.CommandLineParser
{
    /// <summary>
    /// Built-in list of result code.
    /// </summary>
    public enum CommandResultCode
    {
        /// <summary>
        /// The parsing and the option's validation was fine.
        /// </summary>
        Ok = 0,
        /// <summary>
        /// The args parameter of the Parse command is null.
        /// </summary>
        NoArgs = -100,
        /// <summary>
        /// There is no command name in the command-line.
        /// </summary>
        NoCommandName = -200,
        /// <summary>
        /// The requested command was not found.
        /// </summary>
        NoCommandFound = -300,
        /// <summary>
        /// The specified parameter for the options of the command are not valid.
        /// </summary>
        CommandParameterNotValid = -400
    }
}