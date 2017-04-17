using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Defines an base abstraction for the commands. It adds the implementation of the <seealso cref="Arguments" />
    ///     property, and a
    ///     <seealso
    ///         cref="Help" />
    ///     option.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        ///     Initializes a new instance of a <see cref="CommandBase" /> .
        /// </summary>
        protected CommandBase()
        {
            Arguments = new List<string>();
        }

        /// <summary>
        ///     Gets the console used by the parser (if the command needs to writes something).
        /// </summary>
        protected IConsole Console => CurrentDependencyResolverScope.ResolveDependency<IConsole>();

        /// <summary>
        ///     Gets the <see cref="IDependencyResolverScope" /> of the parsing operation.
        /// </summary>
        protected IDependencyResolverScope CurrentDependencyResolverScope { get; private set; }

        /// <summary>
        ///     Gets the <see cref="IParserOptions" /> of the parser.
        /// </summary>
        protected IParserOptions ParserOptions { get; private set; }

        /// <summary>
        ///     Gets the <see cref="CommandType" /> of the command.
        /// </summary>
        protected ICommandType CommandType { get; private set; }

        /// <summary>
        ///     Gets or sets the indicator for showing the help of the command.
        /// </summary>
        [Display(ShortName = "Command_HelpOption_ShortNameMessage",
            Description = "Command_HelpOption_DescriptionMessage", ResourceType = typeof (Strings))]
        [PublicAPI]
        public bool Help { get; set; }

        /// <summary>
        ///     The list of arguments of the command.
        /// </summary>
        public IList<string> Arguments { get; }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
        public virtual int Execute()
        {
            if (Help)
            {
                var helpWriter = CurrentDependencyResolverScope.ResolveDependency<IHelpWriter>();
                helpWriter.WriteHelpForCommand(ParserOptions, CommandType);
                return 0;
            }
            return ExecuteCommand();
        }

        /// <summary>
        ///     Configure the command with the <see cref="IParserOptions" /> and the <see cref="IConsole" /> of the parser.
        /// </summary>
        /// <param name="parserOptions">The <see cref="IParserOptions" />.</param>
        /// <param name="dependencyResolverScope">The <see cref="IDependencyResolverScope" />.</param>
        /// <param name="commandType">The <see cref="CommandType" /> of the command.</param>
        public virtual void Configure(IParserOptions parserOptions, IDependencyResolverScope dependencyResolverScope,
            ICommandType commandType)
        {
            ParserOptions = parserOptions;
            CurrentDependencyResolverScope = dependencyResolverScope;
            CommandType = commandType;
        }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
        protected abstract int ExecuteCommand();
    }
}