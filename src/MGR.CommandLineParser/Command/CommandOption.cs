using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Represents an option of a command.
    /// </summary>
    internal sealed class CommandOption : ICommandOption, ICommandOptionMetadata
    {
        private readonly MethodInfo _miAddMethod;
        private CommandOption(PropertyInfo propertyInfo, ICommandMetadata commandMetadata, List<IConverter> converters)
        {
            PropertyOption = propertyInfo;
            CommandMetadata = commandMetadata;
            DisplayInfo = propertyInfo.ExtractOptionDisplayInfoMetadata();
            Converter = propertyInfo.ExtractConverter(converters, DisplayInfo.Name, CommandMetadata.Name);
            IsRequired = propertyInfo.ExtractIsRequiredMetadata();
            DefaultValue = propertyInfo.ExtractDefaultValue();
            CollectionType = GetMultiValueIndicator(propertyInfo);
            _miAddMethod = PropertyOption.PropertyType.GetMethod("Add");
        }

        /// <summary>
        ///     Gets the display informations of the option.
        /// </summary>
        [NotNull]
        public OptionDisplayInfo DisplayInfo { get; }

        /// <summary>
        ///     Gets the converter for the option.
        /// </summary>
        internal IConverter Converter { get; }

        /// <summary>
        ///     Gets the <see cref="PropertyInfo" /> that represents the option.
        /// </summary>
        internal PropertyInfo PropertyOption { get; }

        /// <summary>
        ///     Gets the command to which the option relates.
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

        /// <summary>
        ///     Convert a value to the expected type of the option.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal object ConvertValue(object value)
        {
            if (value != null)
            {
                if (!value.GetType().IsType(OptionType))
                {
                    var conververtedDefaultValue = Converter.Convert(value.ToString(), OptionType);
                    return conververtedDefaultValue;
                }
            }
            return value;
        }

        public bool OptionalValue => OptionType == typeof(bool);

        public void AssignValue(string optionValue, ICommand command)
        {
            if (!OptionType.IsType(Converter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserSpecifiedConverterNotValidToAssignValue(OptionType, Converter.TargetType));
            }

            if (OptionType == typeof(bool) && optionValue == null)
            {
                optionValue = true.ToString();
            }
            var convertedValue = ConvertValue(optionValue);
            AssignValueInternal(convertedValue, command);
        }

        private void AssignValueInternal(object convertedValue, ICommand command)
        {
            if (!PropertyOption.PropertyType.IsMultiValuedType())
            {
                PropertyOption.SetValue(command, convertedValue, null);
            }
            else
            {
                if (_miAddMethod == null)
                {
                    throw new InvalidOperationException();
                }
                var optionValue = PropertyOption.GetValue(command, null);
                if (optionValue == null)
                {
                    if (PropertyOption.CanWrite)
                    {
                        optionValue = Activator.CreateInstance(PropertyOption.PropertyType);
                        PropertyOption.SetValue(command, optionValue, null);
                    }
                    else
                    {
                        throw new CommandLineParserException(Constants.ExceptionMessages.ParserMultiValueOptionIsNullAndHasNoSetter(DisplayInfo.Name, CommandMetadata.Name));
                    }
                }
                if (PropertyOption.PropertyType.IsCollectionType())
                {
                    _miAddMethod.Invoke(optionValue, new[] { convertedValue });
                }
                else
                {
                    var targetTupleValue = (Tuple<object, object>)convertedValue;
                    _miAddMethod.Invoke(PropertyOption.GetValue(command, null), new[] { targetTupleValue.Item1, targetTupleValue.Item2 });
                }
            }
        }

        internal static CommandOption Create(PropertyInfo propertyInfo, ICommandMetadata commandMetadata, List<IConverter> converters)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));
            Guard.NotNull(commandMetadata, nameof(commandMetadata));
            Guard.NotNull(converters, nameof(converters));

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
            var commandOption = new CommandOption(propertyInfo, commandMetadata, converters);
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