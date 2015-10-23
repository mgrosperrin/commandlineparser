using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Represents an option of a command.
    /// </summary>
    public sealed class CommandOption
    {
        private readonly MethodInfo _miAddMethod ;
        private CommandOption(PropertyInfo propertyInfo, CommandMetadata commandMetadata, IEnumerable<IConverter> converters)
        {
            PropertyOption = propertyInfo;
            CommandMetadata = commandMetadata;
            DisplayInfo = propertyInfo.ExtractOptionDisplayInfoMetadata();
            IsRequired = propertyInfo.ExtractIsRequiredMetadata();
            Converter = propertyInfo.ExtractConverter(converters, DisplayInfo.Name, CommandMetadata.Name);
            DefaultValue = propertyInfo.ExtractDefaultValue(ConvertValue);
            _miAddMethod = PropertyOption.PropertyType.GetMethod("Add");
        }

        /// <summary>
        ///     Gets the display informations of the option.
        /// </summary>
        [NotNull]
        public OptionDisplayInfo DisplayInfo { get; }

        /// <summary>
        ///     Gets the indication that the option is required.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        ///     Gets the converter for the option.
        /// </summary>
        public IConverter Converter { get; }

        /// <summary>
        ///     Gets the <see cref="PropertyInfo" /> that represents the option.
        /// </summary>
        public PropertyInfo PropertyOption { get; }

        /// <summary>
        ///     Gets the command to which the option relates.
        /// </summary>
        public CommandMetadata CommandMetadata { get; }

        /// <summary>
        ///     Gets the default value of the option.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        ///     Gets the underlying type of the option.
        /// </summary>
        public Type OptionType
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
        public object ConvertValue(object value)
        {
            if (value != null)
            {
                if (!value.GetType().IsAssignableFrom(OptionType))
                {
                    var conververtedDefaultValue = Converter.Convert(value.ToString(), OptionType);
                    return conververtedDefaultValue;
                }
            }
            return value;
        }
        internal void AssignValue(string optionValue, ICommand command)
        {
            if (!OptionType.IsAssignableFrom(Converter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserSpecifiedConverterNotValidToAssignValue(OptionType, Converter.TargetType));
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

        internal static CommandOption Create(PropertyInfo propertyInfo, CommandMetadata commandMetadata, IEnumerable<IConverter> converters)
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
    }
}