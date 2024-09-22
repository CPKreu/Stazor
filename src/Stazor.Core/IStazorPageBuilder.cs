using Microsoft.Extensions.DependencyInjection;

namespace Stazor.Core;

public interface IStazorPageBuilder
{
    public IServiceCollection Services { get; }

    public Task<IStazorPageRunner> BuildAsync(string[] arguments);
}