using System.Globalization;

namespace MGR.CommandLineParser.UnitTests;

internal sealed class LangageSwitcher : IDisposable
{
    private readonly CultureInfo _previousCulture;
    private readonly CultureInfo _previousUICulture;

    private LangageSwitcher()
    {
        _previousUICulture = Thread.CurrentThread.CurrentUICulture;
        _previousCulture = Thread.CurrentThread.CurrentCulture;
    }

    internal LangageSwitcher(CultureInfo culture, CultureInfo uiCulture)
        : this()
    {
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = uiCulture;
    }

    internal LangageSwitcher(string culture, string uiCulture)
        : this(new CultureInfo(culture), new CultureInfo(uiCulture))
    {
    }

    internal LangageSwitcher(CultureInfo culture)
        : this()
    {
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
    }

    internal LangageSwitcher(string culture)
        : this(new CultureInfo(culture))
    {
    }

    public void Dispose()
    {
        Thread.CurrentThread.CurrentUICulture = _previousUICulture;
        Thread.CurrentThread.CurrentCulture = _previousCulture;
    }
}