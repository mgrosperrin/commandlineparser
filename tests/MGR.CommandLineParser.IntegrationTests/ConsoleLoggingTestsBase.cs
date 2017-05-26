using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.DependencyInjection;
using MGR.CommandLineParser.UnitTests;

namespace MGR.CommandLineParser.IntegrationTests
{
    public abstract class ConsoleLoggingTestsBase
    {
        static ConsoleLoggingTestsBase()
        {
            DefaultDependencyResolver.RegisterDependency<IConsole>(() =>
                {
                    StringConsole.Current.Reset();
                    return _ => StringConsole.Current;
                }
            );
        }
    }
}