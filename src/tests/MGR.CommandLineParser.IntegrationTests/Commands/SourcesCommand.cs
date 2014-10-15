using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "SourcesCommandDescription", Usage = "SourcesCommandUsageSummary")]
    
    public class SourcesCommand : CommandBase
    {
        [Display(Description = "SourcesCommandNameDescription")]
        public string Name { get; set; }

        [Display(Description = "SourcesCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        [Display(Description = "SourcesCommandUserNameDescription")]
        public string UserName { get; set; }

        [Display(Description = "SourcesCommandPasswordDescription")]
        public string Password { get; set; }

        protected override int ExecuteCommand()
        {
            return 0;
        }
    }
}