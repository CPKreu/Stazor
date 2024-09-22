using Microsoft.AspNetCore.Components;

namespace Stazor.Razor;

public class RazorLayoutTarget(Type? type, RazorLayoutTarget? parent) : IRazorTarget
{
    public Type? RazorType { get; } = type;
    
    public RazorLayoutTarget? Parent { get; } = parent;

    public List<IRazorTarget> Children { get; } = new();

    public RazorLayoutTarget AddPage<T>(string route) where T : IComponent
    {
        var target = new RazorPageTarget(typeof(T), this, route);

        Children.Add(target);
        
        return this;
    }

    public RazorLayoutTarget AddLayout<T>(Action<RazorLayoutTarget> builder) where T : IComponent
    {
        var target = new RazorLayoutTarget(typeof(T), this);

        builder(target);

        Children.Add(target);
        
        return this;
    }
}