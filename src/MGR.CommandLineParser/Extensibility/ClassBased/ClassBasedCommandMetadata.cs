using System;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
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

        public string Name { get; }

        public string Description { get; } = string.Empty;

        public string Usage { get; } = string.Empty;

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] Samples { get; } = new string[0];

        public bool HideFromHelpListing { get; }
    }
}