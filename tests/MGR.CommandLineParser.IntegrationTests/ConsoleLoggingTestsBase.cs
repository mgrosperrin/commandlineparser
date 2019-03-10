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
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IConsole, FakeConsole>();
            serviceCollection.AddCommandLineParser();
            return serviceCollection.BuildServiceProvider().CreateScope().ServiceProvider;
        }
    }
}