using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CommandObjectBuilderBase<TCommandOption> : ICommandObjectBuilder
    where TCommandOption : ICommandOption
    {
        /// <summary>
        ///
        /// </summary>
        protected ICommandMetadata CommandMetadata { get; }
        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<TCommandOption> CommandOptions { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandMetadata"></param>
        /// <param name="commandOptions"></param>
        protected CommandObjectBuilderBase(ICommandMetadata commandMetadata, IEnumerable<TCommandOption> commandOptions)
        {
            CommandMetadata = commandMetadata;
            CommandOptions = commandOptions;
        }

        /// <inheritdoc />
        public abstract void AddArguments(string argument);

        /// <inheritdoc />
        public virtual ICommandOption FindOption(string optionName)
        {
            var commandOption = CommandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
            if (commandOption != null)
            {
                return commandOption;
            }
            var alternateOption = CommandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.AlternateNames.Any(alternateName => alternateName.Equals(optionName, StringComparison.OrdinalIgnoreCase)));
            return alternateOption;
        }

        /// <inheritdoc />
        public virtual ICommandOption FindOptionByShortName(string optionShortName)
        {
            var shortOption = CommandOptions.FirstOrDefault(option => (option.Metadata.DisplayInfo.ShortName ?? string.Empty).Equals(optionShortName, StringComparison.OrdinalIgnoreCase));
            if (shortOption != null)
            {
                return shortOption;
            }
            return FindCombinedBooleanOptionsByShortName(optionShortName);
        }

        /// <inheritdoc />
        public abstract ICommandObject Generate();

        /// <inheritdoc />
        public abstract CommandValidationResult Validate(IServiceProvider serviceProvider);

        /// <summary>
        ///
        /// </summary>
        /// <param name="console"></param>
        /// <param name="results"></param>
        protected void WriteErrorsToConsole(IConsole console, List<ValidationResult> results)
        {
            console.WriteError(Strings.Parser_CommandInvalidArgumentsFormat, CommandMetadata.Name);
            foreach (var validation in results)
            {
                console.WriteError(string.Format(CultureInfo.CurrentUICulture, "-{0} :", validation.ErrorMessage));
                foreach (var memberName in validation.MemberNames)
                {
                    console.WriteError(string.Format(CultureInfo.CurrentUICulture, "  -{0}", memberName));
                }
            }
            console.WriteLine();
        }

        private ICommandOption FindCombinedBooleanOptionsByShortName(string optionShortName)
        {
            var shortName = optionShortName;
            var options = new List<ICommandOption>();
            while (!string.IsNullOrEmpty(shortName))
            {
                var shortOption = CommandOptions.FirstOrDefault(option => !string.IsNullOrEmpty(option.Metadata.DisplayInfo.ShortName) && shortName.StartsWith(option.Metadata.DisplayInfo.ShortName, StringComparison.OrdinalIgnoreCase));
                if (shortOption != null)
                {
                    options.Add(shortOption);
                    shortName = shortName.Substring(shortOption.Metadata.DisplayInfo.ShortName.Length);
                }
                else
                {
                    return null;
                }
            }
            return new WrapCommandOption(optionShortName, CommandMetadata.Name, options);
        }
    }
}