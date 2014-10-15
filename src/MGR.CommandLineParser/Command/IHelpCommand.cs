using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///   Defines the help command.
    /// </summary>
    public interface IHelpCommand : ICommand
    {
        /// <summary>
        ///   Show help for this command.
        /// </summary>
        [Display(ShortName = "Command_HelpOption_ShortNameMessage", Description = "Command_HelpOption_DescriptionMessage", ResourceType = typeof(Strings))]
        bool Help { get; set; }

        /// <summary>
        ///   Show detailled help for all commands.
        /// </summary>
        bool All { get; set; }

        /// <summary>
        ///   Writes help for the specified command. If the command is null, lists all available commands.
        /// </summary>
        /// <param name="command"> The <see cref="ICommand" /> . </param>
        void WriteHelp(ICommand command);

        /// <summary>
        ///   Defines the parser options.
        /// </summary>
        /// <param name="options"> <see cref="IParserOptions" /> The options. </param>
        void DefineOptions(IParserOptions options);
    }
}