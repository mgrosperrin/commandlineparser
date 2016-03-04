using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [CommandDisplay(Description = "PublishCommandDescription", Usage = "PublishCommandUsageDescription")]
    public class PublishCommand : CommandBase
    {
        [Display(Description = "PublishCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        protected override int ExecuteCommand() => 0;
    }
}