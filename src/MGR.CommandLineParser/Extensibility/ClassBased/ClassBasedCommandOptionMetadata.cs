using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    ///     Represents an option of a commandObject.
    /// </summary>
    internal sealed class ClassBasedCommandOptionMetadata : ICommandOptionMetadata
    {
        private ClassBasedCommandOptionMetadata(PropertyInfo propertyInfo, ICommandMetadata commandMetadata, List<IConverter> converters, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            PropertyOption = propertyInfo;
            CommandMetadata = commandMetadata;
            DisplayInfo = propertyInfo.ExtractOptionDisplayInfoMetadata(optionAlternateNameGenerators);
            Converter = propertyInfo.ExtractConverter(converters, DisplayInfo.Name, CommandMetadata.Name);
            IsRequired = propertyInfo.ExtractIsRequiredMetadata();
            DefaultValue = propertyInfo.ExtractDefaultValue();
            CollectionType = GetMultiValueIndicator(propertyInfo);
        }

        /// <summary>
        ///     Gets the display information of the option.
        /// </summary>
        [NotNull]
        public IOptionDisplayInfo DisplayInfo { get; }

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

        public bool IsRequired { get; }
        public CommandOptionCollectionType CollectionType { get; }
        public string DefaultValue { get; }
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
        internal static CommandOptionCollectionType GetMultiValueIndicator(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsDictionaryType())
            {
                return CommandOptionCollectionType.Dictionary;
            }
            if (propertyInfo.PropertyType.IsCollectionType())
            {
                return CommandOptionCollectionType.Collection;
            }
            return CommandOptionCollectionType.None;
        }
    }
}