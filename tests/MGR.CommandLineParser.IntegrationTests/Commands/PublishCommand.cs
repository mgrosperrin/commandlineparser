using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "PublishCommandDescription", Usage = "PublishCommandUsageDescription")]
    public class PublishCommand : CommandBase
    {
        [Display(Description = "PublishCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        protected override int ExecuteCommand() => 0;
    }
}