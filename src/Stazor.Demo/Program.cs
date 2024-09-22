using Stazor.Builder;
using Stazor.Razor;
using Stazor.Servers.AspNet;
using Stazor.Static;

using Stazor.Demo;
using Stazor.Demo.Pages;

var builder = StazorPageBuilder.CreateDefault();

// Used during development to serve the build site
builder.AddAspNetServer();

// Adds the app.cs
builder.AddStaticFiles();

builder.AddRazor(root => root.AddLayout<App>(a =>
{
    a.AddPage<IndexView>("index.html");
    a.AddLayout<SubLayout>(s =>
    {
        s.AddPage<SecondPage>("other.html");
    });
}));

var runner = await builder.BuildAsync(args);

// Runs the commands like build or serve
return await runner.RunAsync();