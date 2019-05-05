//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using JetBrains.Annotations;
//using MGR.CommandLineParser.Command;
//using Microsoft.Extensions.DependencyInjection;

//// ReSharper disable once CheckNamespace
//namespace MGR.CommandLineParser
//{
//    /// <summary>
//    /// Extension's methods for <see cref="IParser"/>.
//    /// </summary>
//    public static class ParserExtensions
//    {
//        private static readonly Lazy<ServiceProvider> ServiceProviderLazy = new Lazy<ServiceProvider>(
//            CreateRootServiceProvider);
//        private static ServiceProvider CreateRootServiceProvider()
//        {
//            var serviceCollection = new ServiceCollection();
//            serviceCollection.AddCommandLineParser().AddClassBasedCommands();
//            var serviceProvider = serviceCollection.BuildServiceProvider();
//            return serviceProvider;
//        }

//        ///// <summary>
//        /////     Parse the supplied arguments for a specific command. The name of the command should not be in the arguments list using the default <see cref="IServiceProvider"/>.
//        ///// </summary>
//        ///// <typeparam name="TCommand">The type of the command.</typeparam>
//        ///// <param name="parser">The current parser.</param>
//        ///// <param name="arguments">The arguments.</param>
//        ///// <remarks>This method can only be used with class-based command.</remarks>
//        ///// <returns>The result of the parsing of the arguments.</returns>
//        //public static ParsingResult Parse<TCommand>(this IParser parser, [ItemNotNull] IEnumerable<string> arguments)
//        //    where TCommand : class, ICommand => parser.Parse<TCommand>(arguments, ServiceProviderLazy.Value.CreateScope().ServiceProvider);

//        ///// <summary>
//        /////     Parse the supplied arguments using the default <see cref="IServiceProvider"/>.
//        ///// </summary>
//        ///// <param name="parser">The current parser.</param>
//        ///// <param name="arguments">The arguments.</param>
//        ///// <returns>The result of the parsing of the arguments.</returns>
//        //public static ParsingResult Parse(this IParser parser, [ItemNotNull] IEnumerable<string> arguments) => parser.Parse(arguments, ServiceProviderLazy.Value.CreateScope().ServiceProvider);

//        ///// <summary>
//        /////     Parse the supplied arguments using the default <see cref="IServiceProvider"/>. If the name of the command is not the first argument, fallback to the specified command. The default command can only be class-based.
//        ///// </summary>
//        ///// <typeparam name="TCommand">The type of the default command.</typeparam>
//        ///// <param name="parser">The current parser.</param>
//        ///// <param name="arguments">The arguments.</param>
//        ///// <returns>The result of the parsing of the arguments.</returns>
//        //[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
//        //public static ParsingResult ParseWithDefaultCommand<TCommand>(this IParser parser, [ItemNotNull] IEnumerable<string> arguments) where TCommand : class, ICommand => parser.ParseWithDefaultCommand<TCommand>(arguments, ServiceProviderLazy.Value.CreateScope().ServiceProvider);
//    }
//}
