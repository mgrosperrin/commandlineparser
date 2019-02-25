using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MGR.CommandLineParser.Command.Lambda
{
    /// <summary>
    /// Represents the object used to build a lambda-based command.
    /// </summary>
    public class CommandBuilder
    {
        private readonly string _commandName;
        private string _description;
        private string _usage;
        private readonly List<string> _samples = new List<string>();
        private bool _hideFromHelpListing;
        private readonly Func<CommandExecutionContext, Task<int>> _executeCommand;
        private readonly List<OptionBuilder> _optionsBuilders = new List<OptionBuilder>();

        internal CommandBuilder(string commandName, Func<CommandExecutionContext, Task<int>> executeCommand)
        {
            _commandName = commandName;
            _executeCommand = executeCommand;
        }

        /// <summary>
        /// Defines the description of the command.
        /// </summary>
        /// <param name="description">The description of the command.</param>
        /// <returns>The <see cref="CommandBuilder"/> to chain calls.</returns>
        public CommandBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        /// <summary>
        /// Defines the usage of the command.
        /// </summary>
        /// <param name="usage">The usage of the command.</param>
        /// <returns>The <see cref="CommandBuilder"/> to chain calls.</returns>
        public CommandBuilder WithUsage(string usage)
        {
            _usage = usage;
            return this;
        }

        /// <summary>
        /// Add a sample to demonstrate the command usage.
        /// </summary>
        /// <param name="sample">A sample.</param>
        /// <returns>The <see cref="CommandBuilder"/> to chain calls.</returns>
        public CommandBuilder AddSample(string sample)
        {
            _samples.Add(sample);
            return this;
        }

        /// <summary>
        /// Hide the command from the help listing.
        /// </summary>
        /// <remarks>This cannot be undone.</remarks>
        /// <returns>The <see cref="CommandBuilder"/> to chain calls.</returns>
        public CommandBuilder HideFromHelpListing()
        {
            _hideFromHelpListing = true;
            return this;
        }

        /// <summary>
        /// Add an option to the command.
        /// </summary>
        /// <typeparam name="T">The type of the option.</typeparam>
        /// <param name="optionName">The name of the option.</param>
        /// <param name="shortOptionName">The short name of the options.</param>
        /// <param name="defineOption">An action to define command</param>
        /// <returns>The <see cref="CommandBuilder"/> to chain calls.</returns>
        public CommandBuilder AddOption<T>(string optionName, string shortOptionName,
            Action<OptionBuilder> defineOption)
        {
            var optionBuilder = new OptionBuilder(optionName, shortOptionName, typeof(T));
            (defineOption ?? (_ => { }))(optionBuilder);
            _optionsBuilders.Add(optionBuilder);
            return this;
        }

        internal LambdaBasedCommandType BuildCommandType(IServiceProvider serviceProvider)
        {
            var commandType = new LambdaBasedCommandType(
                new LambdaBasedCommandMetadata(_commandName, _description, _usage, _samples.ToArray(), _hideFromHelpListing),
                _optionsBuilders.Select(builder => builder.ToCommandOption(serviceProvider)).ToList(), _executeCommand);
            return commandType;
        }
    }
}