namespace Stazor.Core;

public interface IRouteReserve
{
    public string Route { get; }
    
    public IRoutesBuilder Owner { get; }
}