using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

[Command(Description = "PackageCommandDescription", Usage = "PackageCommandUsageSummary")]
public class PackCommand : CommandBase<PackCommand.PackCommandData>
{
    public class PackCommandData : HelpedCommandData
    {
        private static readonly string[] _libPackageExcludes =
                                                                   [
                                                                   @"**\*.pdb",
                                                                   @"src\**\*"
                                                               ];

        private static readonly string[] _symbolPackageExcludes =
                                                                      [
                                                                      @"content\**\*",
                                                                      @"tools\**\*.ps1"
                                                                  ];

        private readonly HashSet<string> _excludes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [Display(Description = "PackageCommandOutputDirDescription")]
        public string OutputDirectory { get; set; }

        [Display(Description = "PackageCommandBasePathDescription")]
        public string BasePath { get; set; }

        [Display(Description = "PackageCommandVerboseDescription", ShortName = "v")]
        public bool Verbose { get; set; }

        [Display(Description = "PackageCommandVersionDescription")]
        public string Version { get; set; }

        [Display(Description = "PackageCommandExcludeDescription")]
        public ICollection<string> Exclude => _excludes;

        [Display(Description = "PackageCommandSymbolsDescription")]
        public bool Symbols { get; set; }

        [Display(Description = "PackageCommandToolDescription", ShortName = "t")]
        public bool Tool { get; set; }

        [Display(Description = "PackageCommandBuildDescription", ShortName = "b")]
        public bool Build { get; set; }
        [Display(Description = "CommandMSBuildVersion")]
        public string MSBuildVersion { get; set; }
        [Display(Description = "PackageCommandNoDefaultExcludes")]
        public bool NoDefaultExcludes { get; set; }

        [Display(Description = "PackageCommandNoRunAnalysis")]
        public bool NoPackageAnalysis { get; set; }

        [Display(Description = "PackageCommandPropertiesDescription")]
        public Dictionary<string, string> Properties => _properties;

        [IgnoreOptionProperty]
        public IEnumerable<object> Rules { get; set; }
    }
    protected override Task<int> ExecuteCommandAsync(PackCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public PackCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}