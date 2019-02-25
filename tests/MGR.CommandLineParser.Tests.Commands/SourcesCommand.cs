using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "SourcesCommandDescription", Usage = "SourcesCommandUsageSummary")]
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

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);
    }
}