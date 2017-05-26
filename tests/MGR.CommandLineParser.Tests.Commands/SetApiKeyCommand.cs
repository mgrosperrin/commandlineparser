using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "SetApiKeyCommandDescription", Usage = "SetApiKeyCommandUsageDescription")]
    public class SetApiKeyCommand : CommandBase
    {
        [Display(Description = "SetApiKeyCommandSourceDescription", ShortName = "src")]
        [DefaultValue("DefaultSource")]
        public string Source { get; set; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; }

        [IgnoreOptionProperty]
        public object Settings { get; }

        protected override int ExecuteCommand() => 0;
    }
}