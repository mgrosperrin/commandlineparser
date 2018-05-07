using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command
{
    internal sealed class Option
    {
        public Option(ICommandOption commandOption, ICommandOptionMetadata commandOptionMetadata)
        {
            CommandOption = commandOption;
            CommandOptionMetadata = commandOptionMetadata;
        }

        public ICommandOption CommandOption { get; }
        public ICommandOptionMetadata CommandOptionMetadata { get; }
    }
}