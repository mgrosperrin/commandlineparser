using System.Collections.Generic;
using System.Data.Common;
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
            var procedures = await _dbContext.Procedures.Include(procedure => procedure.Parameters).ToListAsync();
            return procedures.Select(procedure => MapProcedureToCommandType(procedure, _dbContext.Database.GetDbConnection()));
        }
        public async Task<ICommandType> GetCommandType(string commandName)
        {
            var procedure = await _dbContext.Procedures.Include(procedure => procedure.Parameters).Where(procedure => procedure.Name == commandName).SingleOrDefaultAsync();
            if (procedure != null)
            {
                return MapProcedureToCommandType(procedure, _dbContext.Database.GetDbConnection());
            }
            return null;
        }

        private static CommandType MapProcedureToCommandType(Procedure procedure, DbConnection dbConnection)
        {
            return new CommandType(
                new CommandMetadata(procedure.Name),
                procedure.Parameters
                    .Where(parameter => parameter.Direction.HasFlag(Direction.In))
                    .Select(parameter => new CommandOptionMetadata(
                                new OptionDisplayInfo(parameter.Name),
                                !parameter.HasDefaultValue,
                                parameter.DefaultValue
                                )
                    ),
                procedure.Parameters
                    .Where(parameter => parameter.Direction.HasFlag(Direction.Out)),
                dbConnection
                );
        }
    }
}
