using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

// ReSharper disable CheckNamespace
namespace MGR.CommandLineParser.Tests.Commands
// ReSharper restore CheckNamespace
{
    [CommandDisplay(Description = "InstallCommandDescription", Usage = "InstallCommandUsageSummary")]
    public class InstallCommand : CommandBase
    {
        private ICollection<string> _sources = new List<string>();

        [Display(Description = "InstallCommandSourceDescription")]
        public ICollection<string> Source
        {
            get { return _sources; }
        }

        [Display(Description = "InstallCommandOutputDirDescription")]
        public string OutputDirectory { get; set; }

        [Display(Description = "InstallCommandVersionDescription")]
        public string Version { get; set; }

        [Display(Description = "InstallCommandExcludeVersionDescription", ShortName = "x")]
        public bool ExcludeVersion { get; set; }

        [Display(Description = "InstallCommandPrerelease")]
        public bool Prerelease { get; set; }

        [Display(Description = "InstallCommandNoCache")]
        public bool NoCache { get; set; }

        [IgnoreOptionProperty]
        public object RepositoryFactory { get; private set; }

        [IgnoreOptionProperty]
        public object SourceProvider { get; private set; }

        /// <remarks>
        /// Meant for unit testing.
        /// </remarks>
        [IgnoreOptionProperty]
        protected object CacheRepository
        {
            get { return null; }
        }

        [IgnoreOptionProperty]
        private bool AllowMultipleVersions
        {
            get { return !ExcludeVersion; }
        }

        protected override int ExecuteCommand()
        {
            return 0;
        }
    }
}