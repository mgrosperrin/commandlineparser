using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    [PublicAPI]
    public static class ServiceResolver
    {
        public static IServiceResolver Current { get; private set; } = new DefaultServiceResolver();

        public static void SetServiceResolver(IServiceResolver serviceResolver)
        {
            Current = serviceResolver;
        }
    }
}