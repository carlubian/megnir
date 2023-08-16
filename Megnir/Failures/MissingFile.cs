namespace Megnir.Failures;

public class MissingFile : IFailureMode
{
    public string Name => $"{nameof(MissingFile)}FailureMode";

    public string Cause => $"The file {_name} is missing, or has another problem that prevents it from being accessed.";

    public Exception? Exception { get; }

    private readonly string _name;

    public MissingFile(string fileName)
    {
        _name = fileName;
    }
}
