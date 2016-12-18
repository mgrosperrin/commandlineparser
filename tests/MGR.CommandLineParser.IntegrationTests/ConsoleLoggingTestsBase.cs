namespace MGR.CommandLineParser.IntegrationTests
{
    public abstract class ConsoleLoggingTestsBase
    {
        static ConsoleLoggingTestsBase()
        {
            DefaultDependencyResolver.RegisterDependency<IConsole>(() => _ =>
            {
                MockedConsole.CurrentConsole.Reset();
                return MockedConsole.CurrentConsole;
            });
        }
    }
}