using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "PublishCommandDescription", Usage = "PublishCommandUsageDescription")]
    public class PublishCommand : CommandBase
    {
        [Display(Description = "PublishCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);

        public PublishCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}