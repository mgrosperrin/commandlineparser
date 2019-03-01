using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class CommandBuilder
    {
        private readonly string _commandName;
        private string _description;
        private string _usage;
        private readonly List<string> _samples = new List<string>();
        private bool _hideFromHelpListing;
        private readonly Func<CommandContext, Task<int>> _executeCommand;
        private readonly List<OptionBuilder> _optionsBuilders = new List<OptionBuilder>();

        public CommandBuilder(string commandName, Func<CommandContext, Task<int>> executeCommand)
        {
            _commandName = commandName;
            _executeCommand = executeCommand;
        }
        public CommandBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }
        public CommandBuilder WithUsage(string usage)
        {
            _usage = usage;
            return this;
        }
        public CommandBuilder AddSample(string sample)
        {
            _samples.Add(sample);
            return this;
        }
        public CommandBuilder Hide()
        {
            _hideFromHelpListing = true;
            return this;
        }

        public CommandBuilder AddOption<T>(string optionName, string shortOptionName,
            Action<OptionBuilder> buildAction)
        {
            var optionBuilder = new OptionBuilder(optionName, shortOptionName, typeof(T));
            (buildAction ?? (_ => { }))(optionBuilder);
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