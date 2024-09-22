namespace Stazor.Core;

public interface IStazorServer
{
    public ValueTask<int> ServeAsync(string directory);

    public void NotifyChange(string file);
}