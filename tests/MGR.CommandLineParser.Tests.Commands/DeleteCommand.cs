using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "DeleteCommandDescription", Usage = "DeleteCommandUsageDescription")]
public class DeleteCommand : CommandBase<DeleteCommand.DeleteCommandData>
{
    public class DeleteCommandData : HelpedCommandData
    {
        [Display(Description = "DeleteCommandSourceDescription", ShortName = "src")]
        public string Source { get; set; }

        [Display(Description = "DeleteCommandNoPromptDescription", ShortName = "np")]
        public bool NoPrompt { get; set; }

        [Display(Description = "CommandApiKey")]
        public string ApiKey { get; set; }

        [IgnoreOptionProperty]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object SourceProvider { get; }

        [IgnoreOptionProperty]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object Settings { get; }
    }
    protected override Task<int> ExecuteCommandAsync(DeleteCommandData commandData) => Task.FromResult(0);

    public DeleteCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}