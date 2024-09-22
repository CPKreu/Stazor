namespace Stazor.Builder.Helpers;

internal static class ValueTaskHelpers
{
    public static async ValueTask WhenAll(this IEnumerable<ValueTask> tasks)
    {
        ArgumentNullException.ThrowIfNull(tasks);
        if (!tasks.Any()) return;

        // We don't allocate the list if no task throws
        List<Exception>? exceptions = null;

        foreach (var task in tasks)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions ??= new List<Exception>();
                exceptions.Add(ex);
            }
        }

        if (exceptions is { Count: > 0 })
        {
            throw new AggregateException(exceptions);
        }
    }
}