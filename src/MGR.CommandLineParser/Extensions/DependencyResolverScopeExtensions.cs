// ReSharper disable once CheckNamespace

namespace MGR.CommandLineParser.Command
{
    internal static class DependencyResolverScopeExtensions
    {
        public static HelpCommand GetHelpCommand(this IDependencyResolverScope dependencyResolver, IParserOptions parserOptions)
        {
            var commandTypeProvider = dependencyResolver.ResolveDependency<ICommandTypeProvider>();
            var helpCommandType = commandTypeProvider.GetCommandType(HelpCommand.Name);
            var helpCommand = helpCommandType.CreateCommand(dependencyResolver, parserOptions) as HelpCommand;
            return helpCommand;
        }

        public static void WriteGlobalHelp(this IDependencyResolverScope dependencyResolver, IParserOptions parserOptions)
        {
            var helpCommand = GetHelpCommand(dependencyResolver, parserOptions);
            helpCommand.Execute();
        }
    }
}