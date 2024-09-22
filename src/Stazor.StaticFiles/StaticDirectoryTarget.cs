namespace Stazor.Static;

public class StaticDirectoryTarget(string path)
{
    public string Path { get; } = path;

    public string Wildcard { get; private set; } = "*.*";

    public StaticDirectoryTarget WithWildcard(string wildcard)
    {
        Wildcard = wildcard;

        return this;
    }
}