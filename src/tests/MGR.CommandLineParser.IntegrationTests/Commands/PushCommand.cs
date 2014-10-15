using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "PushCommandDescription", Usage = "PushCommandUsageDescription")]
    public class PushCommand : CommandBase
    {
        [Display(Description = "PushCommandCreateOnlyDescription", ShortName = "co")]
        public bool CreateOnly { get; set; }

        [Display(Description = "PushCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        [Display(Description = "CommandApiKey")]
        public string ApiKey { get; set; }

        [Display(Description = "PushCommandTimeoutDescription")]
        public int Timeout { get; set; }

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