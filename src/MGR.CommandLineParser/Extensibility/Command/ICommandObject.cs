
using System.Threading.Tasks;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    /// Encapsulates a command
    /// </summary>
    public interface ICommandObject
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="argument"></param>
        void AddArguments(string argument);
        /// <summary>
        /// Find an option based on its name.
        /// </summary>
        /// <param name="optionName">The name (short or long form) of the option.</param>
        /// <returns>The <see cref="ICommandOption"/> representing the option of the command.</returns>
        ICommandOption FindOption(string optionName);
        /// <summary>
        /// Find an option based on its short name.
        /// </summary>
        /// <param name="optionShortName">The short name of the option.</param>
        /// <returns>The <see cref="ICommandOption"/> representing the option of the command.</returns>
        ICommandOption FindOptionByShortName(string optionShortName);
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<int> ExecuteAsync();
        /// <summary>
        ///
        /// </summary>
        void Validate();
    }
}
