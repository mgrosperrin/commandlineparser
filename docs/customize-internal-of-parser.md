# Customize the internal of the parser

Some internal parts of the parser can be customized:
the converters for the options, the discovery and the activation of the commands, the generation the help, the "*console*" and the generation of alternate names for the options.

The library uses the dependency injection system provided by [Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).

By default, the library will configure all the services with default implementations when creating the parser by calling `BuildParser()` on a `ParserBuilder` instance.
In this case, the `IServiceProvider` that will be used by the parser will have its own scope.

If you want to customize some services, you have to provide your own `ServiceProvider` when building the parser (by calling the overload `BuildParser(IServiceProvider)`).
Your `IServiceProvider` have to be configured with the services needed by the parser.
This can be done by calling the extension method `AddCommandLineParser` on the `IServiceCollection` that will create the `IServiceProvider`.