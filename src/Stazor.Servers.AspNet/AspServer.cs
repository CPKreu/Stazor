using Microsoft.AspNetCore.Rewrite;
using Stazor.Core;

namespace Stazor.Servers.AspNet;

internal class AspStazorServer : IStazorServer
{
    public async ValueTask<int> ServeAsync(string directory)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            WebRootPath = directory
        });

        builder.Services.AddDirectoryBrowser();
        
        var app = builder.Build();
        
        var rewriter = new RewriteOptions()
            .AddRewrite(@"^([\w\/]+)(?!.)$", "$1.html", skipRemainingRules: true);

        app.UseRewriter(rewriter);

        app.UseFileServer(new FileServerOptions
        {
            EnableDirectoryBrowsing = true,
            StaticFileOptions =
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
            }
        });

        await app.RunAsync();

        return 0;
    }

    public void NotifyChange(string file)
    {
    }
}