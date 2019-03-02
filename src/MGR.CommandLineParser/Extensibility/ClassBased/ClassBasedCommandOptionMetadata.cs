using System;
using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
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
        internal IConverter Converter { get; }

        internal PropertyInfo PropertyOption { get; }

        internal ICommandMetadata CommandMetadata { get; }

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