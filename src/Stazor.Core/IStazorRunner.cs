namespace Stazor.Core;

public interface IStazorPageRunner
{
    public ValueTask<int> RunAsync();
}