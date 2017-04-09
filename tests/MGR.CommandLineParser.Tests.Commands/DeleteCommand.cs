using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "DeleteCommandDescription", Usage = "DeleteCommandUsageDescription")]
    public class DeleteCommand : CommandBase
    {
        [Display(Description = "DeleteCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        [Display(Description = "DeleteCommandNoPromptDescription", ShortName = "np")]
        public bool NoPrompt { get; set; }

        [Display(Description = "CommandApiKey")]
        public string ApiKey { get; set; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; }

        [IgnoreOptionProperty]
        public object Settings { get; }

        protected override int ExecuteCommand() => 0;
    }
}