using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

public class ImportCommand : CommandBase<ImportCommand.ImportCommandData>
{
    public class ImportCommandData : HelpedCommandData
    {
        [Display(ShortName = "p")]
        [DefaultValue(50)]
        public int MaxItemInParallel { get; set; }

        [Display(ShortName = "o")]
        public DirectoryInfo OutputDirectory { get; set; }

        [Display(ShortName = "of")]
        public FileInfo OutputFile { get; set; }
    }
    protected override Task<int> ExecuteCommandAsync(ImportCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(0);

    public ImportCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}