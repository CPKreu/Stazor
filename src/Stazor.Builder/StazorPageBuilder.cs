using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stazor.Builder.Helpers;
using Stazor.Core;

namespace Stazor.Builder;

public class StazorPageBuilder(ServiceCollection services) : IStazorPageBuilder
{
    public ServiceCollection Services { get; } = services;
    public static StazorPageBuilder CreateDefault()
    {
        var services = new ServiceCollection();

        services.AddLogging(b => b.AddConsole());
        
        return new StazorPageBuilder(services);
    }

    IServiceCollection IStazorPageBuilder.Services => Services;

    public async Task<IStazorPageRunner> BuildAsync(string[] arguments)
    {
        // TODO: This could be a bit more advanced
        if (!arguments.Contains("build") && !arguments.Contains("serve"))
        {
            Console.WriteLine("No correct arguments provided. Valid arguments are: [ build, serve ]");
            return new SimpleResultRunner(1);
        }
        
        var services = Services.BuildServiceProvider();
        var logger = services.GetService<ILogger<StazorPageBuilder>>();

        if (logger == null)
        {
            Console.WriteLine("No logging setup.");
        }
        
        var reservedRoutes = new Dictionary<string, IRouteReserve>();

        foreach (var builder in services.GetServices<IRoutesBuilder>())
        {
            foreach (var reserve in builder.ReserveRoutes())
            {
                if (!reservedRoutes.TryAdd(reserve.Route, reserve))
                {
                    logger?.LogError(
                        "{Builder} tried reserving '{Route}' which has already been reserved by {Owner}",
                        builder,
                        reserve.Route,
                        reserve.Owner);
                }
                else
                {
                    logger?.LogDebug("{Builder} reserved '{Route}", builder, reserve.Route);
                }
            }
        }
        
        var buildContext = new BuildContext("obj/stazor/build");
        Directory.CreateDirectory(buildContext.OutputRoot);
        var tasks = new List<ValueTask>();
        
        foreach (var route in reservedRoutes.Values)
        {
            var task = route.Owner.Build(route, buildContext);
            
            logger?.LogInformation("Building '{Route}'", route.Route);
            
            tasks.Add(task);
        }

        await tasks.WhenAll();

        const string finalOutput = "bin/out";
        var fullFinalOutput = Path.GetFullPath(finalOutput);
        Directory.CreateDirectory(fullFinalOutput);

        foreach (var file in Directory.EnumerateFiles(buildContext.OutputRoot, "*.*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(buildContext.OutputRoot, file);
            
            File.Copy(file, Path.Combine(fullFinalOutput, relativePath), true);
            logger?.LogDebug("Copying from {From} to {To}", Path.GetFullPath(file), fullFinalOutput);
        }

        logger?.LogInformation("Build finished, output at {Output}", Path.GetFullPath(finalOutput));

        if (arguments.Contains("serve"))
        {
            return new ServerRunner(fullFinalOutput, services);
        }
        
        return new SimpleResultRunner(0);
    }

    private class SimpleResultRunner(int result) : IStazorPageRunner
    {
        public ValueTask<int> RunAsync() => ValueTask.FromResult(1);
    }

    private class ServerRunner(string outputDirectory, IServiceProvider provider) : IStazorPageRunner
    {
        public async ValueTask<int> RunAsync()
        {
            var server = provider.GetRequiredService<IStazorServer>();
            
            return await server.ServeAsync(outputDirectory);
        }
    }
}
