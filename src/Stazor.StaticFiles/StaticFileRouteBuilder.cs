using Stazor.Core;

namespace Stazor.Static;

internal class StaticFileRouteBuilder(StaticDirectoryTarget target) : IRoutesBuilder<StaticFileReserve>
{
    public IEnumerable<StaticFileReserve> ReserveRoutes() =>
        Directory
            .EnumerateFiles(target.Path, target.Wildcard, SearchOption.AllDirectories)
            .Select(x => new StaticFileReserve(this, GetRoute(x), x));

    public ValueTask Build(StaticFileReserve reserve, BuildContext context)
    {
        File.Copy(reserve.Source, context.GetRoutePath(reserve.Route), true);
        
        return ValueTask.CompletedTask;
    }

    private string GetRoute(string path) => Path.GetRelativePath(target.Path, path);

    public override string ToString() => $"Static files: {target.Path}/{target.Wildcard}";
}
