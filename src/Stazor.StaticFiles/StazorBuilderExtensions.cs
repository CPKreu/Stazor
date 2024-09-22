using Microsoft.Extensions.DependencyInjection;
using Stazor.Core;

namespace Stazor.Static;

public static class StazorBuilderExtensions
{
    public static IStazorPageBuilder AddStaticFiles(this IStazorPageBuilder pageBuilder) => AddStaticFiles(pageBuilder, o => o.FromWwwRoot());
    
    public static IStazorPageBuilder AddStaticFiles(this IStazorPageBuilder pageBuilder, Action<StaticFileBuilderOptions> options)
    {
        var builderOptions = new StaticFileBuilderOptions();
        
        options(builderOptions);
        
        foreach (var target in builderOptions.Targets)
        {
            pageBuilder.Services.AddSingleton<IRoutesBuilder>(_ => new StaticFileRouteBuilder(target));
        }
        
        return pageBuilder;
    }
}