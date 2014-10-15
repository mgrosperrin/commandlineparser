using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using MGR.CommandLineParser.Converters;
using MGR.CommandLineParser.Properties;

// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser
// ReSharper restore CheckNamespace
{
    internal static class ParserOptionsExtensions
    {
        internal static IParserOptions AsReadOnly(this IParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            return new ReadOnlyParserOptions(options);
        }

        internal static ParserOptions ConsolidateOptions(this ParserOptions options)
        {
            options = options ?? new ParserOptions();

            options.Console = options.Console ?? new DefaultConsole();
            options.CommandProvider = options.CommandProvider ?? new DefaultCommandProvider();
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                AssemblyName entryAssemblyName = entryAssembly.GetName();
                options.CommandLineName = options.CommandLineName ?? entryAssemblyName.Name;
                options.Logo = options.Logo ?? string.Format(CultureInfo.CurrentUICulture, Strings.ParserOptions_LogoFormat, entryAssemblyName.Name, entryAssemblyName.Version); 
            }
            options.DefineConverters(new List<IConverter>
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
                                         }, false);
            return options;
        }
    }
}