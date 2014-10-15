using System;
using System.Globalization;
using System.Threading;

namespace MGR.CommandLineParser.UnitTests
{
    internal sealed class LangageSwitcher : IDisposable
    {
        private readonly CultureInfo _previousCulture;

        private LangageSwitcher()
        {
            _previousCulture = Thread.CurrentThread.CurrentUICulture;
        }

        internal LangageSwitcher(CultureInfo culture)
            : this()
        {
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        internal LangageSwitcher(string culture)
            : this(new CultureInfo(culture))
        {
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentUICulture = _previousCulture;
        }
    }
}