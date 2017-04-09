using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser.Command
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Contains extension's methods for <see cref="ICommandTypeProvider" />;
    /// </summary>
    public static class CommandTypeProviderExtensions
    {
        /// <summary>
        ///     Gets all visibles commands (which has not set <see cref="CommandAttribute.HideFromHelpListing" /> to
        ///     <code>false</code>.
        /// </summary>
        /// <param name="commandTypeProvider">The <see cref="ICommandTypeProvider" />.</param>
        /// <returns>The visible <see cref="CommandType" />.</returns>
        public static IEnumerable<CommandType> GetAllVisibleCommandsTypes(this ICommandTypeProvider commandTypeProvider)
        {
            if (commandTypeProvider == null)
            {
                throw new ArgumentNullException(nameof(commandTypeProvider));
            }
            var allCommandTypes = commandTypeProvider.GetAllCommandTypes();
            var visibleCommandTypes = allCommandTypes.Where(commandType => !commandType.Metadata.HideFromHelpListing);
            return visibleCommandTypes;
        }
    }
}