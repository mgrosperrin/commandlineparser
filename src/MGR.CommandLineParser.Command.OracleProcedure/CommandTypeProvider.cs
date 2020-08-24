using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.EntityFrameworkCore;

namespace MGR.CommandLineParser.Command.OracleProcedure
{
    internal class CommandTypeProvider : ICommandTypeProvider
    {
        private readonly OracleSystemContext _dbContext;

        public CommandTypeProvider(OracleSystemContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ICommandType>> GetAllCommandTypes()
        {
            var procedures = _dbContext.Procedures.Include(procedure => procedure.Parameters).ToListAsync();
            return procedures.Select(MapProcedureToCommandType);
        }
        public ICommandType GetCommandType(string commandName)
        {
            var procedure = _dbContext.Procedures.Include(procedure => procedure.Parameters).Where(procedure => procedure.Name == commandName).SingleOrDefault();
            if (procedure != null)
            {
                return MapProcedureToCommandType(procedure);
            }
            return null;
        }

        private static CommandType MapProcedureToCommandType(Procedure procedure) => new CommandType(new CommandMetadata(procedure.Name),
                procedure.Parameters.Where(parameter => parameter.Direction.HasFlag(Direction.In)).Select(parameter => new CommandOptionMetadata(
                    new OptionDisplayInfo(parameter.Name),
                    !parameter.HasDefaultValue,
                    parameter.DefaultValue)),
                procedure.Parameters.Where(parameter => parameter.Direction.HasFlag(Direction.Out));
    }
}
