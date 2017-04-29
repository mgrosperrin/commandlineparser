using MGR.CommandLineParser.UnitTests;

namespace MGR.CommandLineParser.IntegrationTests
{
    public abstract class ConsoleLoggingTestsBase
    {
        static ConsoleLoggingTestsBase()
        {
            DefaultDependencyResolver.RegisterDependency<IConsole>(() => _ => new StringConsole());
        }
    }
}