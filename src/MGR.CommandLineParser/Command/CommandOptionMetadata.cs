using System;
using System.Reflection;
using System.Text;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command
{
    internal class CommandOptionMetadata: ICommandOptionMetadata
    {
        public CommandOptionMetadata(PropertyInfo propertyInfo)
        {
            DisplayInfo =  new OptionDisplayInfo(propertyInfo);
            IsRequired = propertyInfo.ExtractIsRequiredMetadata();
            DefaultValue = propertyInfo.ExtractDefaultValue();
            CollectionType = GetMultiValueIndicator(propertyInfo);
        }
        public bool IsRequired { get; }
        public CommandOptionCollectionType CollectionType { get; }
        public OptionDisplayInfo DisplayInfo { get; }
        public string DefaultValue { get; }

        internal static CommandOptionMetadata Create(PropertyInfo propertyInfo, ICommandMetadata commandMetadata)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));
            Guard.NotNull(commandMetadata, nameof(commandMetadata));

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
            var commandOptionMetadata = new CommandOptionMetadata(propertyInfo);
            return commandOptionMetadata;
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
