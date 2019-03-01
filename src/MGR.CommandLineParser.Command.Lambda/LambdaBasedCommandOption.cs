using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Command.Lambda
{
    internal class LambdaBasedCommandOption : ICommandOption
    {
        internal Type OptionType { get; }
        private readonly IConverter _converter;
        internal ILambdaBasedCommandOptionValueAssigner ValueAssigner { get; }

        public LambdaBasedCommandOption(LambdaBasedComandOptionMetadata commandOptionMetadata, Type optionType, IConverter converter, IEnumerable<ValidationAttribute> validationAttributes)
        {
            OptionType = optionType;
            _converter = converter;
            Metadata = commandOptionMetadata;
            ValidationAttributes = validationAttributes;
            var collectionType = Metadata.CollectionType;
            switch (collectionType)
            {
                case CommandOptionCollectionType.None:
                    ValueAssigner = new LambdaBasedCommandOptionSimpleValueAssigner();
                    break;
                case CommandOptionCollectionType.Collection:
                    ValueAssigner = new LambdaBasedCommandOptionCollectionValueAssigner();
                    break;
                case CommandOptionCollectionType.Dictionary:
                    ValueAssigner = new LambdaBasedCommandOptionDictionnaryValueAssigner();
                    break;
                default:
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one
                    throw new ArgumentOutOfRangeException(nameof(collectionType));
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one
            }

            if (!string.IsNullOrEmpty(Metadata.DefaultValue))
            {
                AssignValue(Metadata.DefaultValue);
            }
        }

        public bool OptionalValue => OptionType == typeof(bool);
        public ICommandOptionMetadata Metadata { get; }
        internal IEnumerable<ValidationAttribute> ValidationAttributes { get; }

        public void AssignValue(string optionValue)
        {
            var value = _converter.Convert(optionValue, OptionType);
            ValueAssigner.AssignValue(value);
        }
    }
}