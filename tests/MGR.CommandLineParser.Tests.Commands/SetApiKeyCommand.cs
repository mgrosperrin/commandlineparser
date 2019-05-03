using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object SourceProvider { get; }

        [IgnoreOptionProperty]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object Settings { get; }

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);

        public SetApiKeyCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}