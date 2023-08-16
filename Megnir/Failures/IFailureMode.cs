namespace Megnir.Failures;

public interface IFailureMode
{
    string Name { get; }
    string Cause { get; }
    Exception? Exception { get; }
}
