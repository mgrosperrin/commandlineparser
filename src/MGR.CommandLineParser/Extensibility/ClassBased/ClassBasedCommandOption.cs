using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased;

internal sealed class ClassBasedCommandOption : ICommandOption
{
    private readonly MethodInfo _miAddMethod;
    private readonly ClassBasedCommandOptionMetadata _commandOptionMetadata;
    private readonly CommandData _commandData;

    internal ClassBasedCommandOption(ClassBasedCommandOptionMetadata commandOptionMetadata, CommandData commandData)
    {
        _commandOptionMetadata = commandOptionMetadata;
        _commandData = commandData;
        _miAddMethod = _commandOptionMetadata.PropertyOption.PropertyType.GetMethod("Add");
        if (!string.IsNullOrEmpty(Metadata.DefaultValue))
        {
            AssignValue(Metadata.DefaultValue);
        }
    }

    public ICommandOptionMetadata Metadata => _commandOptionMetadata;

    private Type OptionType => _commandOptionMetadata.OptionType;

    private object? ConvertValue(object value)
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
        if (convertedValue != null)
        {
            AssignValueInternal(convertedValue);
        }
    }

    private void AssignValueInternal(object convertedValue)
    {
        if (!_commandOptionMetadata.PropertyOption.PropertyType.IsMultiValuedType())
        {
            _commandOptionMetadata.PropertyOption.SetValue(_commandData, convertedValue, null);
        }
        else
        {
            if (_miAddMethod == null)
            {
                throw new InvalidOperationException();
            }
            var optionValue = _commandOptionMetadata.PropertyOption.GetValue(_commandData, null);
            if (optionValue == null)
            {
                if (_commandOptionMetadata.PropertyOption.CanWrite)
                {
                    optionValue = Activator.CreateInstance(_commandOptionMetadata.PropertyOption.PropertyType);
                    _commandOptionMetadata.PropertyOption.SetValue(_commandData, optionValue, null);
                }
                else
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.ParserMultiValueOptionIsNullAndHasNoSetter(_commandOptionMetadata.DisplayInfo.Name, _commandOptionMetadata.CommandMetadata.Name));
                }
            }
            if (_commandOptionMetadata.PropertyOption.PropertyType.IsCollectionType())
            {
                _miAddMethod.Invoke(optionValue, [convertedValue]);
            }
            else
            {
                var targetTupleValue = (KeyValuePair<object, object>)convertedValue;
                _miAddMethod.Invoke(_commandOptionMetadata.PropertyOption.GetValue(_commandData, null), [targetTupleValue.Key, targetTupleValue.Value]);
            }
        }
    }
}