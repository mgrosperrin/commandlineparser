using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Properties;
using Microsoft.Extensions.DependencyInjection;

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
        protected CommandBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Arguments = new List<string>();
        }

        /// <summary>
        ///     Gets the console used by the parser (if the command needs to writes something).
        /// </summary>
        protected IConsole Console => ServiceProvider.GetRequiredService<IConsole>();

        /// <summary>
        ///     Gets the <see cref="IServiceProvider" /> of the parsing operation.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; private set; }

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
        public virtual Task<int> ExecuteAsync()
        {
            if (Help)
            {
                var helpWriter = ServiceProvider.GetRequiredService<IHelpWriter>();
                helpWriter.WriteHelpForCommand(ParserOptions, CommandType);
                return Task.FromResult(0);
            }
            return ExecuteCommandAsync();
        }

        /// <summary>
        ///     Configure the command with the <see cref="IParserOptions" /> and the <see cref="IConsole" /> of the parser.
        /// </summary>
        /// <param name="parserOptions">The <see cref="IParserOptions" />.</param>
        /// <param name="commandType">The <see cref="CommandType" /> of the command.</param>
        public virtual void Configure(IParserOptions parserOptions, ICommandType commandType)
        {
            ParserOptions = parserOptions;
            CommandType = commandType;
        }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
        protected abstract Task<int> ExecuteCommandAsync();
    }
}