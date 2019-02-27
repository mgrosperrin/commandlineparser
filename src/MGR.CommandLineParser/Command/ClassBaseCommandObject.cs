using System.Threading.Tasks;

namespace MGR.CommandLineParser.Command
{
    class ClassBaseCommandObject: ICommandObject, IClassBasedCommandObject
    {
        private readonly ICommand _command;

        public ClassBaseCommandObject(ICommand command)
        {
            _command = command;
        }
        public Task<int> ExecuteAsync() => _command.ExecuteAsync();

        public ICommand Command => _command;
    }
}
