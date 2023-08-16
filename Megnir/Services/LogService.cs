using Pastel;
using Megnir.Failures;
using Snow.Attributes;

namespace Megnir.Services;

[Service]
internal class LogService
{
    internal void Trace(string origin, string message)
    {
        var ts = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}".Pastel("889098");
        var header = " [TRC] ".Pastel("FFFFFF") + origin.Pastel("889098");
        var content = $" {message}".Pastel("889098");

        Console.WriteLine(ts + header + content);
    }

    internal void Info(string origin, string message)
    {
        var ts = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}".Pastel("889098");
        var header = " [INF] ".Pastel("0D98BA") + origin.Pastel("889098");
        var content = $" {message}".Pastel("FFFFFF");

        Console.WriteLine(ts + header + content);
    }

    internal void Warning(string origin, string message)
    {
        var ts = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}".Pastel("889098");
        var header = " [WRN] ".Pastel("DCA310") + origin.Pastel("889098");
        var content = $" {message}".Pastel("FFFFFF");

        Console.WriteLine(ts + header + content);
    }

    internal void Error(string origin, string message)
    {
        var ts = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}".Pastel("889098");
        var header = " [ERR] ".Pastel("BA1010") + origin.Pastel("889098");
        var content = $" {message}".Pastel("FFFFFF");

        Console.WriteLine(ts + header + content);
    }

    internal void FailureMode(IFailureMode mode, bool isCritical = false)
    {
        var ts = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}".Pastel("889098");
        string header;

        if (isCritical)
            header = " [ERR] ".Pastel("BA1010") + mode.Name.Pastel("BA1010");
        else
            header = " [WRN] ".Pastel("DCA310") + mode.Name.Pastel("DCA310");

        var content = $" {mode.Cause}".Pastel("FFFFFF");

        Console.WriteLine(ts + header + content);

        if (isCritical)
            Environment.Exit(1);
    }
}
