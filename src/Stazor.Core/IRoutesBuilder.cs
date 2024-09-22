namespace Stazor.Core;

public interface IRoutesBuilder
{
    IEnumerable<IRouteReserve> ReserveRoutes();
    
    ValueTask Build(IRouteReserve reserve, BuildContext context);
}

public interface IRoutesBuilder<T> : IRoutesBuilder where T : IRouteReserve
{
    IEnumerable<IRouteReserve> IRoutesBuilder.ReserveRoutes() => ReserveRoutes().Cast<IRouteReserve>();

    new IEnumerable<T> ReserveRoutes();

    ValueTask IRoutesBuilder.Build(IRouteReserve reserve, BuildContext context) => Build((T)reserve, context);
    
    ValueTask Build(T reserve, BuildContext context);
}