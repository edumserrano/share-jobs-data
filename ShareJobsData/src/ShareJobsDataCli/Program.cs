namespace ShareJobsDataCli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var app = new ShareJobsDataCli();
        return await app.RunAsync(args);
    }
}
