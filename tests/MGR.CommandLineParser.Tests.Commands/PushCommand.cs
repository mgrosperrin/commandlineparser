using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "PushCommandDescription", Usage = "PushCommandUsageDescription")]
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
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object SourceProvider { get; }

        [IgnoreOptionProperty]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object Settings { get; }

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);

        public PushCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}