using MGR.CommandLineParser.Command.OracleProcedure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal sealed class OracleSystemContext : DbContext
    {
        public DbSet<Procedure> Procedures { get; set; }

        public OracleSystemContext(DbContextOptions<OracleSystemContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ProcedureMap())
                .ApplyConfiguration(new ParameterMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
