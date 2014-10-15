using System;
using System.Collections.Generic;

namespace MGR.CommandLineParser.Converters
{
    internal sealed class KeyValueConverter : IConverter
    {
        private readonly IConverter _keyConverter;
        private readonly IConverter _valueConverter;

        internal KeyValueConverter(IConverter keyConverter, IConverter valueConverter)
        {
            if (keyConverter == null)
            {
                throw new ArgumentNullException("keyConverter");
            }
            if (valueConverter == null)
            {
                throw new ArgumentNullException("valueConverter");
            }
            _keyConverter = keyConverter;
            _valueConverter = valueConverter;
        }

        public Type TargetType
        {
            get { return typeof (KeyValuePair<,>).MakeGenericType(_keyConverter.TargetType, _valueConverter.TargetType); }
        }

        public object Convert(string value, Type concreteTargetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            int eqIndex = value.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            if (eqIndex > -1)
            {
                object propertyKey = _keyConverter.Convert(value.Substring(0, eqIndex), _keyConverter.TargetType);
                object propertyValue = _valueConverter.Convert(value.Substring(eqIndex + 1), _valueConverter.TargetType);
                return Tuple.Create(propertyKey, propertyValue);
            }
            return Tuple.Create(_keyConverter.Convert(value, _keyConverter.TargetType), (object) null);
        }
    }
}