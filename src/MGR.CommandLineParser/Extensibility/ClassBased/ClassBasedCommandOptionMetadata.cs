using System;
using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    ///     Represents an option of a commandObject.
    /// </summary>
    internal sealed class ClassBasedCommandOptionMetadata : CommandOptionMetadataBase
    {
        private ClassBasedCommandOptionMetadata(PropertyInfo propertyInfo, ICommandMetadata commandMetadata, List<IConverter> converters, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
        :base(propertyInfo.ExtractIsRequiredMetadata(),
            GetMultiValueIndicator(propertyInfo.PropertyType),
            propertyInfo.ExtractOptionDisplayInfoMetadata(optionAlternateNameGenerators),
            propertyInfo.ExtractDefaultValue())
        {
            PropertyOption = propertyInfo;
            CommandMetadata = commandMetadata;
            Converter = propertyInfo.ExtractConverter(converters, DisplayInfo.Name, CommandMetadata.Name);
        }

        /// <summary>
        ///     Gets the converter for the option.
        /// </summary>
        internal IConverter Converter { get; }

        /// <summary>
        ///     Gets the <see cref="PropertyInfo" /> that represents the option.
        /// </summary>
        internal PropertyInfo PropertyOption { get; }

        /// <summary>
        ///     Gets the commandObject to which the option relates.
        /// </summary>
        internal ICommandMetadata CommandMetadata { get; }

        /// <summary>
        ///     Gets the underlying type of the option.
        /// </summary>
        internal Type OptionType
        {
            get
            {
                if (PropertyOption.PropertyType.IsMultiValuedType())
                {
                    return PropertyOption.PropertyType.GetUnderlyingCollectionType();
                }
                return PropertyOption.PropertyType;
            }
        }

        internal static ClassBasedCommandOptionMetadata Create(PropertyInfo propertyInfo, ICommandMetadata commandMetadata, List<IConverter> converters, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));
            Guard.NotNull(commandMetadata, nameof(commandMetadata));
            Guard.NotNull(converters, nameof(converters));
            Guard.NotNull(optionAlternateNameGenerators, nameof(optionAlternateNameGenerators));

            if (propertyInfo.ShouldBeIgnored())
            {
                return null;
            }
            if (!propertyInfo.IsValidOptionProperty())
            {
                throw new CommandLineParserException(
                    Constants.ExceptionMessages.ParserExtractMetadataPropertyShouldBeWritableOrICollection(
                        propertyInfo.Name, commandMetadata.Name));
            }
            var commandOption = new ClassBasedCommandOptionMetadata(propertyInfo, commandMetadata, converters, optionAlternateNameGenerators);
            return commandOption;
        }
    }
}