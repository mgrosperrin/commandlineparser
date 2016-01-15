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
            var displayAttribute = commandType.GetAttribute<CommandDisplayAttribute>();
            if (displayAttribute != null)
            {
                _description = displayAttribute.GetLocalizedDescription();
				_usage = displayAttribute.GetLocalizedUsage();
            }
        }

        /// <summary>
        ///     Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the description of the command (if defined).
        /// </summary>
		public string Description { get { return _description; } } 
		string _description = string.Empty;

        /// <summary>
        ///     Gets the usage of the command (if defined).
        /// </summary>
		public string Usage { get { return _usage; } }
		string _usage = string.Empty;
    }
}