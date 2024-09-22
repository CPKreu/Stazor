namespace Stazor.Static;

public class StaticFileBuilderOptions
{
    public List<StaticDirectoryTarget> Targets { get; set; } = new();

    public StaticFileBuilderOptions FromWwwRoot() => FromWwwRoot(_ => { });
    
    public StaticFileBuilderOptions FromWwwRoot(Action<StaticDirectoryTarget> targetOptions) =>
        FromDirectory("wwwroot", targetOptions);

    public StaticFileBuilderOptions FromDirectory(string path) => FromDirectory(path, _ => { });
    
    public StaticFileBuilderOptions FromDirectory(string path, Action<StaticDirectoryTarget> targetOptions)
    {
        var target = new StaticDirectoryTarget(path);

        targetOptions(target);
        Targets.Add(target);

        return this;
    }
}
