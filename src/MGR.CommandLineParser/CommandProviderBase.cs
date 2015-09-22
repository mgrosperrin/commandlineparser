using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Base implementation for the logic of <see cref="ICommandProvider" />. The logic is based on operations on the list of <see cref="ICommand" />.
    /// </summary>
    public abstract class CommandProviderBase : ICommandProvider
    {
        private readonly Lazy<IEnumerable<ICommand>> _commands;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        protected CommandProviderBase()
        {
            _commands = new Lazy<IEnumerable<ICommand>>(BuildCommands);
        }

        /// <inheritdoc />
        public IEnumerable<ICommand> GetAllCommands() => _commands.Value;

        /// <summary>
        ///     Build the list of the <see cref="ICommand" />. This method is called lazily and only once when needed.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected abstract IEnumerable<ICommand> BuildCommands();
    }
}