# Stazor

Stazor is a WIP .NET library for building static web pages using .NET

## Demo

Check out the [demo](src/Stazor.Demo/) for the full thing

```cs
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

return await runner.RunAsync();
```

IndexView.razor
```razor
<h3>Stazor Demo</h3>

<p>Counting to ten: </p>
@for (int i = 0; i < 10; i++)
{
    @((i + 1).ToString()) <br/>
}

<a href="other">Other page ></a>
```