namespace ShareJobsDataCli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT") ?? string.Empty;
        //await using var textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        await using var textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        Console.SetOut(textWriter);
        await textWriter.WriteLineAsync("updated-metrics=21");
        Console.WriteLine("summary-details=123");
        Console.WriteLine($"IsOutputRedirected={Console.IsOutputRedirected}");

        //var app = new ShareDataBetweenJobsCli();
        //return await app.RunAsync(args);
        return 0;
    }
}
