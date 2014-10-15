using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    [DebuggerDisplay("OptionMetadata:{Name}")]
    internal sealed class OptionMetadata
    {
        internal OptionMetadata(OptionMetadataTemplate optionMetadataTemplate, CommandMetadata commandMetadata)
        {
            if (optionMetadataTemplate == null)
            {
                throw new ArgumentNullException("optionMetadataTemplate");
            }
            if (commandMetadata == null)
            {
                throw new ArgumentNullException("commandMetadata");
            }
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

        internal void AssignValue(string option, IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (!OptionType.IsAssignableFrom(Converter.TargetType))
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "The specified converter is not valid : target type is '{1}' and option type is '{0}'.", OptionType,
                                                                   Converter.TargetType));
            }
            object convertedValue = Converter.Convert(option, OptionType);
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
                MethodInfo miAdd = PropertyOption.PropertyType.GetMethod("Add");
                if (miAdd == null)
                {
                    throw new InvalidOperationException();
                }
                object optionValue = PropertyOption.GetValue(Command, null);
                if (optionValue == null)
                {
                    if (PropertyOption.CanWrite)
                    {
                        optionValue = Activator.CreateInstance(PropertyOption.PropertyType);
                        PropertyOption.SetValue(Command, optionValue, null);
                    }
                    else
                    {
                        throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture,
                                                                           "The multi-valued option '{0}' of the command '{1}' returns null and have no setter.", Name,
                                                                           CommandMetadata.Name));
                    }
                }
                if (PropertyOption.PropertyType.IsCollectionType())
                {
                    miAdd.Invoke(optionValue, new[] {convertedValue});
                }
                else
                {
                    Tuple<object, object> targetTupleValue = (Tuple<object, object>) convertedValue;
                    miAdd.Invoke(PropertyOption.GetValue(Command, null), new[] {targetTupleValue.Item1, targetTupleValue.Item2});
                }
            }
        }
    }
}