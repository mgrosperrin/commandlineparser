using System;
using System.Diagnostics;
using System.Reflection;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    [DebuggerDisplay("OptionMetadata:{Name}")]
    internal sealed class OptionMetadata
    {
        internal OptionMetadata(OptionMetadataTemplate optionMetadataTemplate, CommandMetadata commandMetadata)
        {
            Guard.NotNull(optionMetadataTemplate, nameof(optionMetadataTemplate));
            Guard.NotNull(commandMetadata, nameof(commandMetadata));

            Name = optionMetadataTemplate.Name;
            ShortName = optionMetadataTemplate.ShortName;
            Description = optionMetadataTemplate.Description;
            IsRequired = optionMetadataTemplate.IsRequired;
            Converter = optionMetadataTemplate.Converter;
            PropertyOption = optionMetadataTemplate.PropertyOption;
            CommandMetadata = commandMetadata;
            Command = commandMetadata.Command;
            if (optionMetadataTemplate.DefaultValue != null)
            {
                AssignValueInternal(optionMetadataTemplate.DefaultValue);
            }
        }

        internal string Name { get; set; }
        internal string ShortName { get; set; }
        internal string Description { get; set; }
        internal bool IsRequired { get; set; }
        internal IConverter Converter { get; set; }
        internal PropertyInfo PropertyOption { get; set; }
        internal CommandMetadata CommandMetadata { get; set; }
        internal ICommand Command { get; set; }

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

        internal void AssignValue(string option)
        {
            if (!OptionType.IsAssignableFrom(Converter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserSpecifiedConverterNotValidToAssignValue(OptionType, Converter.TargetType));
            }
            var convertedValue = Converter.Convert(option, OptionType);
            AssignValueInternal(convertedValue);
        }

        private void AssignValueInternal(object convertedValue)
        {
            if (!PropertyOption.PropertyType.IsMultiValuedType())
            {
                PropertyOption.SetValue(Command, convertedValue, null);
            }
            else
            {
                var miAdd = PropertyOption.PropertyType.GetMethod("Add");
                if (miAdd == null)
                {
                    throw new InvalidOperationException();
                }
                var optionValue = PropertyOption.GetValue(Command, null);
                if (optionValue == null)
                {
                    if (PropertyOption.CanWrite)
                    {
                        optionValue = Activator.CreateInstance(PropertyOption.PropertyType);
                        PropertyOption.SetValue(Command, optionValue, null);
                    }
                    else
                    {
                        throw new CommandLineParserException(Constants.ExceptionMessages.ParserMultiValueOptionIsNullAndHasNoSetter(Name, CommandMetadata.Name));
                    }
                }
                if (PropertyOption.PropertyType.IsCollectionType())
                {
                    miAdd.Invoke(optionValue, new[] {convertedValue});
                }
                else
                {
                    var targetTupleValue = (Tuple<object, object>) convertedValue;
                    miAdd.Invoke(PropertyOption.GetValue(Command, null), new[] {targetTupleValue.Item1, targetTupleValue.Item2});
                }
            }
        }
    }
}