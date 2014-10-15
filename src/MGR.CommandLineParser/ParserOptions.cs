using System;
using System.Linq;
using System.Collections.Generic;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Default implementation of <see cref="IParserOptions"/>.
    /// </summary>
    public sealed class ParserOptions : IParserOptions
    {
        private readonly List<IConverter> _converters = new List<IConverter>();
        /// <summary>
        /// The implementation of <see cref="IConsole"/> used by the parser.
        /// </summary>
        public IConsole Console { get; set; }
        /// <summary>
        /// The implementation of <see cref="ICommandProvider"/> used by the parser.
        /// </summary>
        public ICommandProvider CommandProvider { get; set; }
        /// <summary>
        /// The logo used in the help by the parser.
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// The name of the executable to run used in the help by the parser.
        /// </summary>
        public string CommandLineName { get; set; }
        /// <summary>
        /// The collection of <see cref="IConverter"/> used by the parser.
        /// </summary>
        public IEnumerable<IConverter> Converters { get { return _converters.AsEnumerable(); } }
        /// <summary>
        /// Define new converters for the parser.
        /// </summary>
        /// <remarks>For each converter, if this <see cref="ParserOptions"/> already have a registred converter with the same TargetType, it will be overwrited.</remarks>
        /// <param name="converters">The new converters.</param>
        /// <returns>This instance of <see cref="ParserOptions"/>.</returns>
        public ParserOptions DefineConverters(IEnumerable<IConverter> converters)
        {
            return DefineConverters(converters, true /* Default Value */);
        }
        /// <summary>
        /// Define new converters for the parser.
        /// </summary>
        /// <param name="converters">The new converters.</param>
        /// <param name="overwrite">If true, will overwrite every converter with the same TargetType.</param>
        /// <returns>This instance of <see cref="ParserOptions"/>.</returns>
        public ParserOptions DefineConverters(IEnumerable<IConverter> converters, bool overwrite)
        {
            if (converters != null)
            {
                foreach (var converter in converters)
                {
                    DefineConverter(converter, overwrite);
                }
            }
            return this;
        }
        /// <summary>
        /// Define new converter for the parser.
        /// </summary>
        /// <remarks>If this <see cref="ParserOptions"/> already have a registred converter with the same TargetType, it will be overwrited.</remarks>
        /// <param name="converter">The new converter.</param>
        /// <returns>This instance of <see cref="ParserOptions"/>.</returns>
        public ParserOptions DefineConverter(IConverter converter)
        {
            return DefineConverter(converter, true /* Default Value */);
        }
        /// <summary>
        /// Define new converter for the parser.
        /// </summary>
        /// <param name="converter">The new converter.</param>
        /// <param name="overwrite">If true, will overwrite the converter with the same TargetType.</param>
        /// <returns>This instance of <see cref="ParserOptions"/>.</returns>
        public ParserOptions DefineConverter(IConverter converter, bool overwrite)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            IConverter oldConverter = _converters.FirstOrDefault(conv => conv.CanConvertTo(converter.TargetType));
            if(oldConverter == null)
            {
                _converters.Add(converter);
            }
            else if(overwrite)
            {
                _converters.Remove(oldConverter);
                _converters.Add(converter);
            }
            return this;
        }
    }
}