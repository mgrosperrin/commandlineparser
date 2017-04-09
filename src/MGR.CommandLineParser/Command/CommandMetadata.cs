using System;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Represents the metadata of a command.
    /// </summary>
    public sealed class CommandMetadata
    {
        internal CommandMetadata(Type commandType)
        {
            Name = commandType.GetFullCommandName();
            var displayAttribute = commandType.GetAttribute<CommandAttribute>();
            if (displayAttribute != null)
            {
                Description = displayAttribute.GetLocalizedDescription();
                Usage = displayAttribute.GetLocalizedUsage();
            }
        }

        /// <summary>
        ///     Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the description of the command (if defined).
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        ///     Gets the usage of the command (if defined).
        /// </summary>
        public string Usage { get; } = string.Empty;
    }
}