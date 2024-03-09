using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Tests.Commands;

public class IntTestCommand : CommandBase<IntTestCommand.IntTestCommandData>
{
    public class IntTestCommandData : HelpedCommandData
    {
        [Display(ShortName = "s", Description = "A simple string value")]
        [Required]
        public string StrValue { get; set; }
        [Display(ShortName = "i", Description = "A simple integer value")]
        public int IntValue { get; set; }
        [Display(ShortName = "b", Description = "A boolean value")]
        public bool BoolValue { get; set; }
        [Display(ShortName = "il", Description = "A list of integer value")]
        public List<int> IntListValue { get; set; }
    }

    protected override Task<int> ExecuteCommandAsync(IntTestCommandData commandData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IntTestCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}