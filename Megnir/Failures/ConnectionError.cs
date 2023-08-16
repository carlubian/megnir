namespace Megnir.Failures;

public class ConnectionError : IFailureMode
{
    public string Name => $"{nameof(ConnectionError)}FailureMode";

    public string Cause => "Connection to Azure is off, or the connection string is incorrect.";

    public Exception? Exception { get; }
}
