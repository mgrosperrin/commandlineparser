using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MGR.CommandLineParser.Command.OracleProcedure.Mapping
{
    internal class ParameterMap : IEntityTypeConfiguration<Parameter>
    {
        public void Configure(EntityTypeBuilder<Parameter> builder)
        {
            builder.ToTable("USER_ARGUMENTS");

            builder.Property(_ => _.Name)
                .HasColumnName("OBJECT_NAME");

            var defaultValueConverter = new BoolToStringConverter("N", "Y");
            builder.Property(_ => _.HasDefaultValue)
                .HasColumnName("DEFAULTED")
                .HasConversion(defaultValueConverter);

            builder.Property(_ => _.DefaultValue)
                .HasColumnName("DEFAULT_VALUE");

            builder.Property(_ => _.DataType)
                .HasColumnName("DATA_TYPE");

            var directionConverter = new ValueConverter<Direction, string>(
                _ => _.ToString(),
                value => ParseDirection(value));
            builder.Property(_ => _.Direction)
                .HasColumnName("IN_OUT")
                .HasConversion(directionConverter);
        }
        private static Direction ParseDirection(string value) => value
                switch
        {
            "IN" => Direction.In,
            "OUT" => Direction.Out,
            "IN/OUT" => Direction.InOut,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}