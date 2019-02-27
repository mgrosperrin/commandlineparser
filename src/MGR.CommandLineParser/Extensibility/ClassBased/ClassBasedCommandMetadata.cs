using System;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    ///     Represents the metadata of a command.
    /// </summary>
    internal sealed class ClassBasedCommandMetadata : ICommandMetadata
    {
        internal ClassBasedCommandMetadata(Type commandType)
        {
            Name = commandType.GetFullCommandName();
            var commandAttribute = commandType.GetAttribute<CommandAttribute>();
            if (commandAttribute != null)
            {
                Description = commandAttribute.GetLocalizedDescription();
                Usage = commandAttribute.GetLocalizedUsage();
                Samples = commandAttribute.Samples ?? new string[0];
                HideFromHelpListing = commandAttribute.HideFromHelpListing;
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

        /// <summary>
        ///     Gets the samples for the command.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] Samples { get; } = new string[0];
        /// <summary>
        /// Determine if the command should be hidden from the help listing.
        /// </summary>
        public bool HideFromHelpListing { get; }
    }
}