using System.Collections.Generic;

namespace MGR.CommandLineParser.Converters
{
    internal static class Converters
    {
        internal static List<IConverter> GetAll() => new List<IConverter>
        {
            new BooleanConverter(),
            new ByteConverter(),
            new CharConverter(),
            new DateTimeConverter(),
            new DecimalConverter(),
            new DoubleConverter(),
            new EnumConverter(),
            new GuidConverter(),
            new Int16Converter(),
            new Int32Converter(),
            new Int64Converter(),
            new SingleConverter(),
            new StringConverter(),
            new TimeSpanConverter(),
            new UriConverter()
        };
    }
}