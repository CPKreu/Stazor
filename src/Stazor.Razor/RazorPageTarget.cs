namespace Stazor.Razor;

public class RazorPageTarget(Type razorType, RazorLayoutTarget layout, string route) : IRazorTarget
{
    public Type RazorType { get; } = razorType;

    public RazorLayoutTarget Layout { get; set; } = layout;
    
    public string Route { get; } = route;
}