using Stazor.Core;

namespace Stazor.Servers.AspNet;

public static class StazorBuilderExtensions
{
    public static IStazorPageBuilder AddAspNetServer(this IStazorPageBuilder builder)
    {
        builder.Services.AddSingleton<IStazorServer, AspStazorServer>();
        
        return builder;
    }
}