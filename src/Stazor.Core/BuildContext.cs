namespace Stazor.Core;

public class BuildContext(string outputRoot)
{
    public string OutputRoot => outputRoot;
    
    public Stream GetRouteStream(string route) => new FileStream(GetRoutePath(route), FileMode.Create);
    
    public string GetRoutePath(string route) => Path.Combine(outputRoot, route);
}