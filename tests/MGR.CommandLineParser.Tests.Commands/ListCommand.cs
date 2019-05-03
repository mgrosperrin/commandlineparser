using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands
{
    [Command(Description = "ListCommandDescription", Usage = "ListCommandUsageDescription", Samples = new []{"List sample 1", "List sample number 2"})]
    public class ListCommand : CommandBase
    {
        private readonly List<string> _sources = new List<string>();

        [Display(Description = "ListCommandSourceDescription")]
        public ICollection<string> Source => _sources;

        [Display(Description = "ListCommandVerboseListDescription")]
        public bool Verbose { get; set; }

        [Display(Description = "ListCommandAllVersionsDescription")]
        public bool AllVersions { get; set; }

        [Display(Description = "ListCommandPrerelease")]
        public bool Prerelease { get; set; }

        [IgnoreOptionProperty]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object RepositoryFactory { get; }

        [IgnoreOptionProperty]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object SourceProvider { get; }

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(0);

        public ListCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}