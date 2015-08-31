using System;
using System.Globalization;
using System.Reflection;
using System.Runtime;

namespace MGR.CommandLineParser.Command
{
    internal sealed class LocalizableString
    {
        private Func<string> _cachedResult;
        private readonly string _propertyName;
        private string _propertyValue;
        private Type _resourceType;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal LocalizableString(string propertyName)
        {
            _propertyName = propertyName;
        }

        private void ClearCache()
        {
            _cachedResult = null;
        }

        internal string GetLocalizableValue()
        {
            if (_cachedResult == null)
            {
                if ((_propertyValue == null) || (_resourceType == null))
                {
                    _cachedResult = () => _propertyValue;
                }
                else
                {
                    var property = _resourceType.GetProperty(_propertyValue);
                    var flag = false;
                    if ((!_resourceType.IsVisible || (property == null)) || (property.PropertyType != typeof (string)))
                    {
                        flag = true;
                    }
                    else
                    {
                        var getMethod = property.GetGetMethod();
                        if (((getMethod == null) || !getMethod.IsPublic) || !getMethod.IsStatic)
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        var exceptionMessage = string.Format(CultureInfo.CurrentCulture,
                                                                "Cannot retrieve property '{0}' because localization failed.  Type '{1}' is not public or does not contain a public static string property with the name '{2}'",
                                                                new object[] {_propertyName, _resourceType.FullName, _propertyValue});
                        _cachedResult = delegate { throw new InvalidOperationException(exceptionMessage); };
                    }
                    else
                    {
                        _cachedResult = () => (string)property.GetValue(null, null);
                    }
                }
            }
            return _cachedResult();
        }

        internal Type ResourceType
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get { return _resourceType; }
            set
            {
                if (_resourceType != value)
                {
                    ClearCache();
                    _resourceType = value;
                }
            }
        }

        internal string Value
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get { return _propertyValue; }
            set
            {
                if (_propertyValue != value)
                {
                    ClearCache();
                    _propertyValue = value;
                }
            }
        }
    }
}