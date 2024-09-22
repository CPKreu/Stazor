using Stazor.Core;

namespace Stazor.Razor;

internal class RazorPageReserve(RazorPageRouteBuilder owner, string route, RazorPageTarget target) : IRouteReserve
{
    public IRoutesBuilder Owner { get; } = owner;
    
    public string Route { get; } = route;

    public RazorPageTarget Target { get; } = target;
}