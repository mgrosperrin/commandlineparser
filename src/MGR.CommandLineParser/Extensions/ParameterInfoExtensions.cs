namespace System.Reflection;

internal static class ParameterInfoExtensions
{
    public static bool TryGetDefaultValue(this ParameterInfo parameter, out object? defaultValue)
    {
        var useDefaultValueFromParameterInfo = true;
        defaultValue = null;
        bool hasDefaultValue;
        try
        {
            hasDefaultValue = parameter.HasDefaultValue;
        }
        catch (FormatException) when (parameter.ParameterType == typeof(DateTime))
        {
            hasDefaultValue = true;
            useDefaultValueFromParameterInfo = false;
        }
        if (hasDefaultValue)
        {
            if (useDefaultValueFromParameterInfo)
            {
                defaultValue = parameter.DefaultValue;
            }

            if (defaultValue == null && parameter.ParameterType.IsValueType)
            {
                defaultValue = Activator.CreateInstance(parameter.ParameterType);
            }
        }
        return hasDefaultValue;
    }
}
