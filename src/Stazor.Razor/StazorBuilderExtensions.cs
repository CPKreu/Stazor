using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Stazor.Core;

namespace Stazor.Razor;

public static class StazorBuilderExtensions
{
    public static IStazorPageBuilder AddRazor(this IStazorPageBuilder builder, Action<RazorLayoutTarget> target)
    {
        builder.Services.AddSingleton<IRoutesBuilder>(s =>
        {
            var layoutTarget = new RazorLayoutTarget(null, null);
        
            target(layoutTarget);

            return new RazorPageRouteBuilder(s, layoutTarget);
        });
        
        return builder;
    }
}