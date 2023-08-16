namespace Megnir.Model;

internal class Success
{
    private static readonly Success _instance = new Success();

    public override int GetHashCode() => 1;

    public override bool Equals(object? obj) => obj is Success;

    public override string ToString() => "Success";

    public static Success Instance => _instance;
}
