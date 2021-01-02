using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGR.CommandLineParser.Command.OracleProcedure.Mapping
{
    internal class ProcedureMap : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder.ToTable("ALL_OBJETS");

            builder.Property(_ => _.Name)
                .HasColumnName("OBJECT_NAME");

            builder.HasQueryFilter(_ => EF.Property<string>(_, "OBJECT_TYPE") == "PROCEDURE");
        }
    }
}