﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandType : ICommandType
    {
        private readonly IEnumerable<LambdaBasedCommandOption> _options;
        private readonly Func<CommandExecutionContext, Task<int>> _executeCommand;

        internal LambdaBasedCommandType(LambdaBasedCommandMetadata commandMetadata, IEnumerable<LambdaBasedCommandOption> options, Func<CommandExecutionContext, Task<int>> executeCommand)
        {
            _options = options;
            _executeCommand = executeCommand;
            Metadata = commandMetadata;
        }

        public ICommandMetadata Metadata { get; }

        public IEnumerable<ICommandOptionMetadata> Options => _options.Select(option => option.Metadata);

        public ICommandObjectBuilder CreateCommandObjectBuilder(IServiceProvider serviceProvider) => new LambdaBasedCommandObjectBuilder(Metadata, _options, serviceProvider, _executeCommand);
    }
}
