using System;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.IntegrationTests
{
    public abstract class ConsoleLoggingTestsBase
    {
        protected static IServiceProvider CreateServiceProvider()
        {
            StringConsole.Current.Reset();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandLineParser()
                .AddTransient<IConsole>(_ => StringConsole.Current);
            return serviceCollection.BuildServiceProvider().CreateScope().ServiceProvider;
        }
    }
}