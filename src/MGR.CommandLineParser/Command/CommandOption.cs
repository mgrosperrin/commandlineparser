using System;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Command
{
    /// <summary>
    ///     Represents an option of a commandObject.
    /// </summary>
    internal sealed class CommandOption : ICommandOption
    {
        private readonly MethodInfo _miAddMethod;
        private readonly ClassBasedCommandOptionMetadata _commandOptionMetadata;
        private readonly ICommand _command;

        internal CommandOption(ClassBasedCommandOptionMetadata commandOptionMetadata, ICommand command)
        {
            _commandOptionMetadata = commandOptionMetadata;
            _command = command;
            _miAddMethod = _commandOptionMetadata.PropertyOption.PropertyType.GetMethod("Add");
            if (!string.IsNullOrEmpty(Metadata.DefaultValue))
            {
                AssignValue(Metadata.DefaultValue);
            }
        }

        public ICommandOptionMetadata Metadata => _commandOptionMetadata;

        /// <summary>
        ///     Gets the underlying type of the option.
        /// </summary>
        private Type OptionType => _commandOptionMetadata.OptionType;

        /// <summary>
        ///     Convert a value to the expected type of the option.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ConvertValue(object value)
        {
            if (value != null && !value.GetType().IsType(OptionType))
            {
                var conververtedDefaultValue = _commandOptionMetadata.Converter.Convert(value.ToString(), OptionType);
                return conververtedDefaultValue;
            }
            return value;
        }

        public bool OptionalValue => OptionType == typeof(bool);

        public void AssignValue(string optionValue)
        {
            if (!OptionType.IsType(_commandOptionMetadata.Converter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserSpecifiedConverterNotValidToAssignValue(OptionType, _commandOptionMetadata.Converter.TargetType));
            }

            if (OptionType == typeof(bool) && optionValue == null)
            {
                optionValue = true.ToString();
            }
            var convertedValue = ConvertValue(optionValue);
            AssignValueInternal(convertedValue);
        }

        private void AssignValueInternal(object convertedValue)
        {
            if (!_commandOptionMetadata.PropertyOption.PropertyType.IsMultiValuedType())
            {
                _commandOptionMetadata.PropertyOption.SetValue(_command, convertedValue, null);
            }
            else
            {
                if (_miAddMethod == null)
                {
                    throw new InvalidOperationException();
                }
                var optionValue = _commandOptionMetadata.PropertyOption.GetValue(_command, null);
                if (optionValue == null)
                {
                    if (_commandOptionMetadata.PropertyOption.CanWrite)
                    {
                        optionValue = Activator.CreateInstance(_commandOptionMetadata.PropertyOption.PropertyType);
                        _commandOptionMetadata.PropertyOption.SetValue(_command, optionValue, null);
                    }
                    else
                    {
                        throw new CommandLineParserException(Constants.ExceptionMessages.ParserMultiValueOptionIsNullAndHasNoSetter(_commandOptionMetadata.DisplayInfo.Name, _commandOptionMetadata.CommandMetadata.Name));
                    }
                }
                if (_commandOptionMetadata.PropertyOption.PropertyType.IsCollectionType())
                {
                    _miAddMethod.Invoke(optionValue, new[] { convertedValue });
                }
                else
                {
                    var targetTupleValue = (Tuple<object, object>)convertedValue;
                    _miAddMethod.Invoke(_commandOptionMetadata.PropertyOption.GetValue(_command, null), new[] { targetTupleValue.Item1, targetTupleValue.Item2 });
                }
            }
        }
    }
}