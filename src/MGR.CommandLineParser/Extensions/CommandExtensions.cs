using System;
// ReSharper disable CheckNamespace

namespace MGR.CommandLineParser.Command
// ReSharper restore CheckNamespace
{
    internal static class CommandExtensions
    {
        internal static string ExtractCommandName(this ICommand source)
        {
            Guard.NotNull(source, nameof(source));

            return source.GetType().ExtractCommandMetadataTemplate().Name;
        }

        internal static CommandMetadata ExtractMetadata(this ICommand source)
        {
            Guard.NotNull(source, nameof(source));
            return source.GetType().ExtractMetadataTemplate().ToCommandMetadata(source);
        }
    }
}