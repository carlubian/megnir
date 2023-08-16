namespace Megnir.Failures;

public class InvalidFormat : IFailureMode
{
    public string Name => $"{nameof(InvalidFormat)}FailureMode";

    public string Cause => $"The file {_name} has the wrong format or cannot be parsed. It was expected to have {_format} format.";

    public Exception? Exception { get; }

    private readonly string _name;
    private readonly string _format;

    public InvalidFormat(string fileName, string expectedFormat)
    {
        _name = fileName;
        _format = expectedFormat;
    }
}
