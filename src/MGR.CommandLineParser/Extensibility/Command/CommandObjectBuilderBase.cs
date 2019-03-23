using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Properties;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///Represents a basic implementation for <see cref="ICommandObjectBuilder"/> that provide implementation for retrieving <see cref="ICommandOption"/> (potentially combined for boolean options) and validation (especially writing of validation errors).
    /// </summary>
    public abstract class CommandObjectBuilderBase<TCommandOption> : ICommandObjectBuilder
    where TCommandOption : ICommandOption
    {
        /// <summary>
        ///Gets the metadata of the command being build.
        /// </summary>
        protected ICommandMetadata CommandMetadata { get; }

        /// <summary>
        ///Gets the list of options of the command being build.
        /// </summary>
        protected IEnumerable<TCommandOption> CommandOptions { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandObjectBuilderBase{TCommandOption}"/>.
        /// </summary>
        /// <param name="commandMetadata">The command metadata.</param>
        /// <param name="commandOptions">The command's options.</param>
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
            var commandOption = CommandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.Name.Equals(optionName, StringComparison.Ordinal));
            if (commandOption != null)
            {
                return commandOption;
            }
            var alternateOption = CommandOptions.FirstOrDefault(option => option.Metadata.DisplayInfo.AlternateNames.Any(alternateName => alternateName.Equals(optionName, StringComparison.Ordinal)));
            return alternateOption;
        }

        /// <inheritdoc />
        public virtual ICommandOption FindOptionByShortName(string optionShortName)
        {
            var shortOption = CommandOptions.FirstOrDefault(option => (option.Metadata.DisplayInfo.ShortName ?? string.Empty).Equals(optionShortName, StringComparison.Ordinal));
            if (shortOption != null)
            {
                return shortOption;
            }
            return FindCombinedBooleanOptionsByShortName(optionShortName);
        }

        /// <inheritdoc />
        public abstract ICommandObject GenerateCommandObject();

        /// <inheritdoc />
        public virtual CommandValidationResult Validate(IServiceProvider serviceProvider)
        {
            var results = new List<ValidationResult>();
            var isCommandValid = DoValidate(results, serviceProvider);
            if (!isCommandValid)
            {
                var console = serviceProvider.GetRequiredService<IConsole>();
                WriteErrorsToConsole(console, results);
            }
            return new CommandValidationResult(isCommandValid, results);
        }

        /// <summary>
        /// Perform the validation of the command.
        /// </summary>
        /// <param name="validationResults">The list of validation errors to populate.</param>
        /// <param name="serviceProvider">The current <see cref="IServiceProvider"/> that can be used to gets services.</param>
        /// <returns><code>true</code> if the command being build is valid, <code>false</code> elsewhere.</returns>
        protected virtual bool DoValidate(List<ValidationResult> validationResults, IServiceProvider serviceProvider) => true;

        /// <summary>
        /// Write the validation errors to the current <see cref="IConsole"/>.
        /// </summary>
        /// <param name="console">The <see cref="IConsole"/> to write to.</param>
        /// <param name="results">the validation errors to write to the <see cref="IConsole"/>.</param>
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
                var shortOption = CommandOptions.FirstOrDefault(option => !string.IsNullOrEmpty(option.Metadata.DisplayInfo.ShortName) && shortName.StartsWith(option.Metadata.DisplayInfo.ShortName, StringComparison.Ordinal));
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
            return new CombinedCommandOption(optionShortName, CommandMetadata.Name, options);
        }
    }
}