namespace ShareJobsDataCli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var app = new ShareDataBetweenJobsCli();
        return await app.RunAsync(args);
    }
}
