using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "PublishCommandDescription", Usage = "PublishCommandUsageDescription")]
public class PublishCommand : CommandBase<PublishCommand.PublishCommandData>
{
    public class PublishCommandData : HelpedCommandData
    {
        [Display(Description = "PublishCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }
    }
    protected override Task<int> ExecuteCommandAsync(PublishCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public PublishCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}