using System;
using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal sealed class ClassBasedCommandOption : ICommandOption
    {
        private readonly MethodInfo _miAddMethod;
        private readonly ClassBasedCommandOptionMetadata _commandOptionMetadata;
        private readonly ICommand _command;

        internal ClassBasedCommandOption(ClassBasedCommandOptionMetadata commandOptionMetadata, ICommand command)
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

        private Type OptionType => _commandOptionMetadata.OptionType;

        private object ConvertValue(object value)
        {
            if (value != null && !value.GetType().IsType(OptionType))
            {
                var convertedDefaultValue = _commandOptionMetadata.Converter.Convert(value.ToString(), OptionType);
                return convertedDefaultValue;
            }
            return value;
        }

        public bool ShouldProvideValue => OptionType != typeof(bool);

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
                    var targetTupleValue = (KeyValuePair<object, object>)convertedValue;
                    _miAddMethod.Invoke(_commandOptionMetadata.PropertyOption.GetValue(_command, null), new[] { targetTupleValue.Key, targetTupleValue.Value });
                }
            }
        }
    }
}