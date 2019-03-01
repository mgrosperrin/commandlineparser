using System;
using System.Collections.Generic;
using System.Linq;

namespace MGR.CommandLineParser.Command.Lambda
{
    public class CommandContext
    {
        private readonly IEnumerable<LambdaBasedCommandOption> _commandOptions;

        internal CommandContext(IEnumerable<LambdaBasedCommandOption> commandOptions, List<string> arguments, IServiceProvider serviceProvider)
        {
            _commandOptions = commandOptions;
            Arguments = arguments;
            ServiceProvider = serviceProvider;
        }

        public IEnumerable<string> Arguments { get; }
        public T GetOption<T>(string name)
        {
            var option = _commandOptions.FirstOrDefault(o => o.Metadata.DisplayInfo.Name == name);
            if (option == null)
            {
                throw new InvalidOperationException();
            }

            if (!typeof(T).IsAssignableFrom(option.OptionType))
            {
                throw new InvalidOperationException();
            }

            var rawValue = option.ValueAssigner.GetValue();
            if (rawValue == null)
            {
                return default(T);
            }

            return (T) rawValue;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}