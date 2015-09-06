using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "SetApiKeyCommandDescription", Usage = "SetApiKeyCommandUsageDescription")]
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