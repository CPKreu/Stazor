using Stazor.Core;

namespace Stazor.Static;

internal class StaticFileReserve(StaticFileRouteBuilder builder, string route, string source) : IRouteReserve
{
    public string Route { get; } = route;

    public IRoutesBuilder Owner { get; } = builder;

    public string Source { get; } = source;
}