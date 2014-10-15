using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///   Defines an base abstraction for the commands. It adds the implementation of the <seealso cref="Arguments" /> property, and a <seealso
    ///    cref="Help" /> option.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        private IConsole _console;

        /// <summary>
        ///   Gets the console used by the parser (if the command needs to writes something).
        /// </summary>
        protected IConsole Console
        {
            get { return _console; }
        }

        /// <summary>
        ///   Initializes a new instance of a <see cref="CommandBase" /> .
        /// </summary>
        protected CommandBase()
        {
            Arguments = new List<string>();
        }

        /// <summary>
        ///   The list of arguments of the command.
        /// </summary>
        public IList<string> Arguments { get; private set; }

        /// <summary>
        ///   Gets or sets the indicator for showing the help of the command.
        /// </summary>
        [Display(ShortName = "Command_HelpOption_ShortNameMessage", Description = "Command_HelpOption_DescriptionMessage", ResourceType = typeof (Strings))]
        public bool Help { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
        public virtual int Execute()
        {
            if (Help)
            {
                HelpCommand.Current.WriteHelp(this);
                return 0;
            }
            return ExecuteCommand();
        }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
        protected abstract int ExecuteCommand();

        internal void DefineConsole(IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            _console = console;
        }
    }
}