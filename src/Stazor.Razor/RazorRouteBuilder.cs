using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stazor.Core;

namespace Stazor.Razor;

internal class RazorPageRouteBuilder(IServiceProvider services, RazorLayoutTarget target) : IRoutesBuilder<RazorPageReserve>
{
    private HtmlRenderer _htmlRenderer = new(services, services.GetRequiredService<ILoggerFactory>());
    
    public IEnumerable<RazorPageReserve> ReserveRoutes()
    {
        var pages = new List<RazorPageReserve>();

        AddChildren(pages, target);

        return pages;
    }

    public async ValueTask Build(RazorPageReserve reserve, BuildContext context)
    {
        var fragments = GetRenderFragments(reserve.Target, out var final);
        
        var dictionary = new Dictionary<string, object?>
        {
            { "Body", fragments[^1] }
        };

        var parameters = ParameterView.FromDictionary(dictionary);

        await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await _htmlRenderer.RenderComponentAsync(final.RazorType!, parameters);

            using var outputStream = context.GetRouteStream(reserve.Route);
            using var writer = new StreamWriter(outputStream);

            output.WriteHtmlTo(writer);
        });
    }

    private void AddChildren(List<RazorPageReserve> targets, RazorLayoutTarget parent)
    {
        foreach (var child in parent.Children)
        {
            switch (child)
            {
                case RazorPageTarget page:
                    targets.Add(new RazorPageReserve(this, page.Route, page));
                    break;
                case RazorLayoutTarget layoutTarget:
                    AddChildren(targets, layoutTarget);
                    break;
            }
        }
    }

    private List<RazorLayoutTarget> GetFragmentLayouts(RazorPageTarget page, out RazorLayoutTarget final)
    {
        var layouts = new List<RazorLayoutTarget>();
        
        var layout = page.Layout;
        while (layout is { RazorType: not null })
        {
            layouts.Add(layout);
            layout = layout.Parent;
        }

        final = layouts[^1];
        layouts.RemoveAt(layouts.Count - 1);
        
        return layouts;
    }

    private List<RenderFragment> GetRenderFragments(RazorPageTarget page, out RazorLayoutTarget final)
    {
        var layouts = GetFragmentLayouts(page, out final);
        
        var fragments = new List<RenderFragment>();

        var child = CreateFragment(page.RazorType, null);
        fragments.Add(child);

        foreach (var layout in layouts)
        {
            child = CreateFragment(layout.RazorType!, child);
            fragments.Add(child);
        }

        return fragments;

        RenderFragment CreateFragment(Type razorType, RenderFragment? body) => b =>
        {
            b.OpenComponent(0, razorType);

            if (body != null)
            {
                b.AddComponentParameter(0, "Body", body);
            }
            
            b.CloseComponent();
        };
    }
}