using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "DeleteCommandDescription", Usage = "DeleteCommandUsageDescription")]
    public class DeleteCommand : CommandBase
    {
        [Display(Description = "DeleteCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        [Display(Description = "DeleteCommandNoPromptDescription", ShortName = "np")]
        public bool NoPrompt { get; set; }

        [Display(Description = "CommandApiKey")]
        public string ApiKey { get; set; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; private set; }

        [IgnoreOptionProperty]
        public object Settings { get; private set; }

        protected override int ExecuteCommand()
        {
            return 0;
        }
    }
}